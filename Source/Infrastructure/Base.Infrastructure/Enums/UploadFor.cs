namespace Base.Infrastructure.Enums;
public static class UploadFor
{
#if DEBUG
    public const string VoiceRecordingsNas = "A:\\Cati";
    public const string VoiceRecordings = "Z:";
#else
        public const string VoiceRecordingsNas = "/mnt/nas";
        public const string VoiceRecordings = "/var/lib/freeswitch/recordings/";
#endif
    public const string VoiceExtensionWav = ".wav";
    public const string VoiceExtensionOgg = ".ogg";
    public const string VoiceExtensionMp3 = ".mp3";
}