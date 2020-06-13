using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CalloutAPI;
/* This callout is one of the first callouts that I have made. I also consider this open source. If anybody wants to offer suggestions or add to the code...
 * I would be more than accepting of it. */


namespace IndecentExposure
{
    [CalloutProperties("IndecentExposure","Shplink","1.0",Callout.Probability.Medium)]
    public class IndecentExposure : CalloutAPI.Callout
    {

        Ped Suspect, Victim;
        private bool dialogueFinished = false;
        private PedHash[] suspectHashList = new PedHash[] { PedHash.Topless01AFY, PedHash.Stripper02Cutscene, PedHash.Bodybuild01AFM, PedHash.Acult01AMM, PedHash.Acult02AMY, PedHash.Musclbeac01AMY };

        public IndecentExposure()
        {
            Random random = new Random();
            float offsetX = random.Next(100, 600);
            float offsetY = random.Next(100, 600);

            InitBase(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));

            this.ShortName = "Indecent Exposure";
            this.CalloutDescription = "A report of somebody indecently exposing themselves has come in. Respond Code 1.";
            this.ResponseCode = 1;
            this.StartDistance = 30f;
        }

        public async override Task Init()
        {
            OnAccept();

            Random random = new Random();

            var suspectPed = suspectHashList[random.Next(suspectHashList.Length)];

            Suspect = await SpawnPed(suspectPed, Location);
            Victim = await SpawnPed(GetRandomPed(), Location);

            Suspect.AlwaysKeepTask = true;
            Suspect.BlockPermanentEvents = true;
            Victim.AlwaysKeepTask = true;
            Victim.BlockPermanentEvents = true;
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);

            Conversate();
            if (dialogueFinished == true)
            {
                Suspect.Task.FightAgainst(Victim);
                Victim.Task.ReactAndFlee(Suspect);
            }
        }

        private void Conversate()
        {
            DrawSubtitle("~r~[SUSPECT]~w~ Hey! Take a look at this!", 5000);
            Wait(5000);
            DrawSubtitle("~b~[VICTIM]~w~ Get away you pervert!", 5000);
            dialogueFinished = true;
        }

        private void Notify(string message)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(message);
            DrawNotification(false, false);
        }

        private void DrawSubtitle(string message, int duration)
        {
            BeginTextCommandPrint("STRING");
            AddTextComponentSubstringPlayerName(message);
            EndTextCommandPrint(duration, false);
        }
    }
}
