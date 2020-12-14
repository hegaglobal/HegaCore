using System.Collections.Generic;

namespace HegaCore.Events.Commands
{
    using Data;

    public class DataCommandRegisterer : CommandRegisterer
    {
        protected sealed override void Register(bool @override)
        {
            if (UnityEngine.SingletonBehaviour.Quitting)
                return;

            Register(GetCoreCommands(), @override);
            Register(GetCommands(), @override);
        }

        private IEnumerable<DataCommand> GetCoreCommands()
        {
            yield return new CharacterClipClear();
            yield return new CharacterClipUnlock();
            yield return new CharacterImageClear();
            yield return new CharacterImageUnlock();
            yield return new CharacterProgressSet();
            yield return new CharacterProgressUnlock();
            yield return new ConversationSet();
            yield return new ConversationShowEnd();
            yield return new ConversationShowStart();
            yield return new MissionClear();
            yield return new MissionPass();
            yield return new MissionPassedClear();
            yield return new MissionPendingClear();
            yield return new MissionPending();
            yield return new MissionUnlock();
            yield return new MusicPlay();
            yield return new MusicPlayAsync();
            yield return new MusicSetLoop();
            yield return new MusicStop();
            yield return new PointAdd();
            yield return new PointBadAdd();
            yield return new PointGoodAdd();
            yield return new PointMissionAdd();
            yield return new PrefabDestroy();
            yield return new PrefabDestroyAll();
            yield return new PrefabHide();
            yield return new PrefabHideAll();
            yield return new PrefabShow();
            yield return new SoundPlay();
            yield return new SoundPlayAsync();
            yield return new SoundStop();
            yield return new VoiceBackgroundPlay();
            yield return new VoiceBackgroundPlayAsync();
            yield return new VoiceBackgroundSetLoop();
            yield return new VoiceBackgroundStop();
            yield return new VoicePlay();
            yield return new VoicePlayAsync();
            yield return new VoiceStop();
            yield return new VoiceStopAll();
        }
    }
}