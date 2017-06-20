using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// SOUND ID BANK
public enum SoundClipId
{
	SFX_LOG_DROP,
	ERROR
}
	
public class AudioController : MonoBehaviour
{
	Dictionary<SoundClipId, string> clips;


	private static readonly string PrefKey_MuteSFX = "muteSFX";
	private static readonly string PrefKey_MuteBGM = "muteBGM";

	static private bool _muteBGM = false;
	static private bool _muteSFX = false;

	private static AudioController singleton;

	static float bgmVol;

	public AudioSource bgm;

	public static AudioController getSingleton()
	{
		if (singleton == null)
		{
			
			GameObject go = new GameObject("| AudioController |");
			singleton = go.AddComponent<AudioController>();
			singleton.TryInit();

		}
			
		return singleton;
	}
		
	void Start(){

		if(singleton != this){
			Debug.LogError("Extra Audio Controller: "+ gameObject.name, gameObject);
			Destroy(this);
		}
	}

	void TryInit(){

			DontDestroyOnLoad(gameObject);

			/*
			 * CHECK USER PREFS
			 */
			_muteBGM = PlayerPrefsUtil.readBoolPref(PrefKey_MuteBGM, false); // isIntTrue(PlayerPrefs.GetInt("muteBGM" , 0));
			_muteSFX = PlayerPrefsUtil.readBoolPref(PrefKey_MuteSFX, false);



			if(_muteSFX){
				bgmVol = 0.0f;
			}else{
				bgmVol = 1.0f;
			}


			this.SetupSoundClips();
			singleton = this;

	}

	void SetupSoundClips(){

		clips = new Dictionary<SoundClipId, string>();

		clips.Add( SoundClipId.SFX_LOG_DROP, "Sounds/hitwall" );
		clips.Add( SoundClipId.ERROR, "Sounds/hitwall" );

		//Debug.Log("SOUND CLIPS DIC SETUP");

	}

	public void muteSFX(bool setMuteSFX)
	{
		if (setMuteSFX == _muteSFX)
		{
			return;
		}
		//if(_muteSFX)
		_muteSFX = setMuteSFX;
		PlayerPrefsUtil.writeBoolPref(PrefKey_MuteSFX, _muteSFX);
		if (!_muteSFX)
		{
			// let user know sound effects are one
			AudioController.getSingleton().PlaySFX("Sounds/Tiny Button Push-SoundBible.com-513260752");
		}

		sync();
	}

	public bool getMuteSFX(){
		return _muteSFX;
	}

	public bool getMuteBGM(){
		return _muteBGM;
	}


	void OnDestroy()
	{
		if (singleton == this)
		{
			singleton = null;
		}
	}
		
	public void PlaySFX( string url , float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{
		StartCoroutine(CoroutinePlaySFX(url, volume, pitch, delay));
	}

	public void PlaySFX( SoundClipId id , float volume = 1.0f, float pitch = 1.0f, float delay = 0)
	{
		// Get url based on key
		string url = GetSoundClip(id);


		StartCoroutine(CoroutinePlaySFX(url, volume, pitch, delay));
	}

	public void PlaySFXRandomFromArray(string[] arrayOfUrls, float volume = 1.0f, float pitch = 1.0f, float delay = 0){

		string name = arrayOfUrls[Random.Range(0, arrayOfUrls.Length )];
		AudioController.getSingleton().PlaySFX(name, volume, pitch, delay);
	}

	string GetSoundClip(SoundClipId id){

		string url;

		if(clips.ContainsKey(id))
		{
			url = clips[id];
		}else
		{
			Debug.LogError("Sound not found id= " + id);
			url = clips[SoundClipId.ERROR];
		}

		return url;
	}

	IEnumerator CoroutinePlaySFX(string url , float volume, float pitch, float delay){
		//stuff
		yield return new WaitForSeconds (Time.timeScale * delay);

		if(_muteSFX)
		{ 
			// It's muted
			//return;
		}
		else
		{
			
			AudioClip ac  = Resources.Load(url) as AudioClip;

			if(ac == null){

				Debug.Log("MISSING Sound effect url");
			//return;
			}else{

				GameObject sfx = new GameObject(); // create the temp object
				sfx.transform.position = Vector3.zero; // set its position
				AudioSource aSource = sfx.AddComponent<AudioSource>(); // add an audio source
				aSource.clip = ac; // define the clip
				aSource.volume = volume;
				aSource.Play(); // start the sound
				aSource.pitch = pitch;
				DontDestroyOnLoad(sfx);
				Destroy(sfx, ac.length + 0.5f); // destroy object after clip duration
			}
		}
	}


	//static GameObject this;

	public void PlayBG( string url , float volume = 1.0f, bool loop = true)
	{
		AudioClip ac  = Resources.Load(url) as AudioClip;

		if(ac == null){

			Debug.Log("MISSING Sound effect url");
			return;
		}



		//bg = new GameObject(); // create the temp object
		//bg.transform.position = Vector3.zero; // set its position
		//bgm = bgm; // add an audio source
		if(bgm == null){
			bgm = this.gameObject.AddComponent<AudioSource>(); 
		}
		bgm.clip = ac; // define the clip
		bgm.loop = loop;
		bgm.volume = bgmVol;
		bgm.Play(); // start the sound
		//DontDestroyOnLoad(bg);

		//Destroy(sfx, ac.length + 0.5f); // destroy object after clip duration
	}

	public void StopBG(){
		if(bgm){
			bgm.Stop();
		}
		//if(bg){

			//Destroy(bg);
		//}
	}

	void sync()
	{
		bgmVol = (_muteSFX) ? 0.0f : 1.0f;
		bgm.volume = bgmVol;
	}


}