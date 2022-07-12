using System;
using System.IO;
using System.Text;
using System.Linq;

public class CPHInline
{

    /* Step 1
     *  Create a ReplayBuffer Clip
     * Step 2 
     *  Save it on PC
     * Step 3 
     *  Send ReplayBuffer Clip to Discord Channel
     */
    public bool Execute()
    {

        string yourReplayPath = "D:\\Videos\\";
        string yourOutputPath = "D:\\Videos\\Clips\\";

        string discordWebhook = args["webhook"].ToString();
        string user = args["userName"].ToString();
        string title = args["rawInput"].ToString();

        // If there's no input from the chatter, it won't save the clip.
        if (String.IsNullOrEmpty(title))
        {
            var errMsg = user + ", you must specify a name for the clip.";
            CPH.SendMessage(errMsg);
            return true;
        }

        CPH.ObsReplayBufferSave();
        System.Threading.Thread.Sleep(2000);
        // Scan the initial folder where the clip is saved for the newest file created - presumably the clip
        var clipBackups = new DirectoryInfo(@"" + yourReplayPath + "");
        // Filter by MKV files - or change to whatever file output format you have your replay buffer configured to.
        var clipFile = clipBackups.GetFiles("*.mkv").OrderByDescending(p => p.CreationTime).FirstOrDefault();
        // If no file found, quit out.
        if (clipFile == null){
            var errMsg = args["userName"].ToString() + ", the clip couldn't be saved.";
            CPH.SendMessage(errMsg);
            return true;
        }

        // Grab the file name by the Windows Path
        var clipPath = clipFile.FullName;
        // Rename the file
        string newFileName = @"" + yourOutputPath + title + " (clipped by " + user + ").mkv";
        System.IO.File.Move(clipPath, newFileName);

        return true;
    }
}