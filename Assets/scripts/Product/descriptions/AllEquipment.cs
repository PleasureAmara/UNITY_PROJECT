using NUnit.Framework.Constraints;
using System.Collections.Generic;

using localizer.core.enums;

namespace localizer.product.descriptions
{

    public class ToDoModel
    {
        public string ControllerKey { get; set; }
        public string Description { get; set; } 
    }

    public class ControllerKeyDescription
    {
        public TargetControllerKeys nextTargetControllerKey { get; set; }
        public string Description { get; set; }
    }
    public static class ActionsDescriptions
    {
        private  readonly static Dictionary<TutorialStep, ControllerKeyDescription> _allActionsDict = new Dictionary<TutorialStep, ControllerKeyDescription>
        {
            [TutorialStep.WelcomeToVR] = new ControllerKeyDescription { nextTargetControllerKey = TargetControllerKeys.Select, Description = "Hello, You're going to be taken through a few steps to make sure you're ready to navigate yourself in the next upcoming scenes. Press the glowing button on your controller." },
            [TutorialStep.LearningGrip] = new ControllerKeyDescription { nextTargetControllerKey = TargetControllerKeys.SnapTurn, Description = "The pressed button is used for clicking buttons, switches and gripping the door handles. You point the controller to the target and press. For doors press, hold and pull or push to open or close. Try the next glowing button."},
            [TutorialStep.LearningTurning] = new ControllerKeyDescription { nextTargetControllerKey = TargetControllerKeys.Move, Description = "The pressed button is used for turning around without moving your body. The turn provided is a snap turn to improve user experience.Try the next glowing button." },
            [TutorialStep.LearningStraightMovt] = new ControllerKeyDescription {Description = "The pressed thumbstick is used for moving in straight lines. i.e. forward, backwards, left and right. Press 'Grip' button to continue, (The button used for selecting)." },
            [TutorialStep.FinishVRControllerLearning] = new ControllerKeyDescription { Description = "Now you're ready for action. You're going to be teleported to the airside vehicle. Find a comfortable seat and enjoy your learning. Once ready, Point the controller to the CONTINUE button and press the 'Grip' button." },
            [TutorialStep.Quit] = new ControllerKeyDescription { Description = "To quit, Press the button with a meta logo on the right controller . A Menu will appear, choose the QUIT APP option." },
        };

        public static string FindDescription(TutorialStep searchKey, out TargetControllerKeys relatedButton)
        {
            if (_allActionsDict.TryGetValue(searchKey, out ControllerKeyDescription matchingDescription))
            {
                relatedButton = matchingDescription.nextTargetControllerKey;
                return matchingDescription.Description;
            }
            relatedButton = TargetControllerKeys.Empty;
            return string.Empty; 
        }
        //public static string FindDescription(string searchKey, out string relatedButton)
        //{
        //    if (_allActionsDictionary.TryGetValue(searchKey, out ToDoModel matchingDescription))
        //    {
        //        relatedButton = matchingDescription.ControllerKey;
        //        return matchingDescription.Description;
        //    }
        //    relatedButton = null;
        //    return string.Empty;
        //}
    }
}
