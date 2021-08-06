namespace HegaCore.Events.Commands
{
    using Data;

    public static class CoreDataCommands
    {
        public static readonly DataCommand[] Commands = new DataCommand[]
        {
            new CharacterClipClear(),
            new CharacterClipUnlock(),
            new CharacterImageClear(),
            new CharacterImageUnlock(),
            new CharacterProgressSet(),
            new CharacterProgressUnlock(),

            new ConversationSet(),
            new ConversationShowEnd(),
            new ConversationShowStart(),

            new MissionClear(),
            new MissionPass(),
            new MissionPassedClear(),
            new MissionPendingClear(),
            new MissionPending(),
            new MissionUnlock(),

            new PointAdd(),
            new PointBadAdd(),
            new PointGoodAdd(),
            new PointMissionAdd(),

            new PrefabDestroy(),
            new PrefabDestroyAll(),
            new PrefabHide(),
            new PrefabHideAll(),
            new PrefabShow(),

            new MusicPlay(),
            new MusicPlayAsync(),
            new MusicPlayData(),
            new MusicPlayDataAsync(),
            new MusicSetLoop(),
            new MusicStop(),

            new SoundPlay(),
            new SoundPlayAsync(),
            new SoundPlayData(),
            new SoundPlayDataAsync(),
            new SoundStop(),

            new VoiceBackgroundPlay(),
            new VoiceBackgroundPlayAsync(),
            new VoiceBackgroundSetLoop(),
            new VoiceBackgroundStop(),

            new VoicePlay(),
            new VoicePlayAsync(),
            new VoiceStop(),
            new VoiceStopAll(),
        };
    }
}