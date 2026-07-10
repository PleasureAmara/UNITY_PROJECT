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
        //private readonly static Dictionary<string, ToDoModel> _allActionsDictionary = new Dictionary<string, ToDoModel>
        //{
        //    ["Welcome to VR"] = new ToDoModel { Description ="Hello, You're going to be taken through a few steps to make sure you're ready to navigate yourself around the next upcoming scenes. So seat back, relax and hold your VR controllers in both hands."},
        //    ["Clicking buttons and switches"] = new ToDoModel { ControllerKey = "Select", Description = "Point the controller to the button or switch of interest, once the ray from the controller turns orange, press the grip button once. (Grip Button is the big button on the inner side of the controller where the folding fingers rest). Try pressing the button to confirm you're pressing the correct one." },
        //    ["Straight locomotion"] = new ToDoModel {ControllerKey = "Move",  Description = "With your left controller, push the thumbstick forward to glide forward (Thumbstick is the tallest button on the top of the controller and can be pushed around in a 360 degree movement), backwards to glide back, and sideways to move either to the left or right. Try pressing the button to confirm you're pressing the correct one." },
        //    ["Rotating Around"] = new ToDoModel {ControllerKey = "SnapTurn", Description = "With your right controller, push the thumbstick to the right or left to turn accordingly. Note: To avoid nausea, we provide snap turning. Try pressing the button to confirm you're pressing the correct one." },
        //    ["Opening and closing Doors"] = new ToDoModel {ControllerKey = "Select", Description = "Point the controller to the door knob, the ray from the controller will turn orange once it detects the knob, hold the grip button and move your hand as if you are pulling or pushing the door. Try pressing the button to confirm you're pressing the correct one." },
        //    ["Final Conclusion"] = new ToDoModel { Description = "Thanks for completing the introduction tutorial, now lets proceed with the next tutorial." },
        //    ["openShelterDoorText"] = new ToDoModel { Description = "To open the Localizer door, come closer and grab the door handle with your left hand, then pull it." },
        //    ["manageShelterLights"] = new ToDoModel { Description = "Once you've entered, on your left, there is a switch, press it to turn on the lights, and again to turn off." },
        //    ["openLZZFrontDoor"] = new ToDoModel { Description = "Now face the front of the localizer and grab the front door handle and pull to open the door." },
        //};

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
