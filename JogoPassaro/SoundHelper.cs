using Plugin.Maui.Audio;

public class SoundHelper
{
    public static object AudioManager { get; private set; }

    //------------------------------------------------------------------------

    public static void Play(string nomeArquivoWav, bool loop = false)
  {
    Task.Run(async () =>
    {
      var audioFX = await FileSystem.OpenAppPackageFileAsync(nomeArquivoWav);
      var audioPlayer = AudioManager.Current.CreatePlayer(audioFX);
      audioPlayer.Loop = loop;
      audioPlayer.Play();
    });
  }

  //------------------------------------------------------------------------

}