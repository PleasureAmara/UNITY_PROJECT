using System.Collections.Generic;

namespace localizer.product.descriptions
{

    public class ActionsModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    } 
    public static class ActionsDescriptions
    {
        private static Dictionary<string, string> _allActionsDictionary = new Dictionary<string, string>
        {
            {"Welcome to VR", "Hello there, You're gonna be taken through a few steps to make sure you're ready to navigate yourself around the next upcoming scenes. So seat back, relax and hold your VR controllers in both hands." },
            {"Straight locomotion", "With your left controller, push the thumbstick forward to glide forward (Thumbstick is the tallest button on the top of the controller and can be pushed around in a 360 degree movement), backwards to glide back, and sideways to move either to the left or right."},
            {"Rotating Around", "With your right controller, push the thumbstick to the right or left to turn accordingly. Note: To avoid nausea, we provide snap turning."},
            {"Opening and closing Doors", "Point the controller to the door knob, the ray from the controller will turn orange once it detects the knob, hold the grip button (This is the big button on the inner side of the controller where the folding fingers rest.) move your hand as if you are pulling or pushing the door."},
            {"Clicking buttons and switches","Point the controller to the button or switch of interest, once the ray from the controller turns orange, press the grip button once." },
            {"Final Conclusion", "Thanks for completing the introduction tutorial, now lets proceed with the next tutorial."},
            {"openShelterDoorText", "To open the Localizer door, come closer and grab the door handle with your left hand, then pull it."},
            {"manageShelterLights", "Once you've entered, on your left, there is a switch, press it to turn on the lights, and again to turn off."},
            {"openLZZFrontDoor", "Now face the front of the localizer and grab the front door handle and pull to open the door."}

        };

        public static string FindDescription(string searchKey)
        {
            if (_allActionsDictionary.TryGetValue(searchKey, out string matchingDescription))
            {
                return matchingDescription;
            }
            return string.Empty;
        }

        //public static ActionsModel[] _allActionsArray = new ActionsModel[]
        //{
            //new ActionsModel()
            //{
            //    Name = "Welcome to VR",
            //    Description = "Hello there, You're gonna be taken through a few steps to make sure you're ready to navigate yourself around the next upcoming scenes. So seat back, relax and hold your VR controllers in both hands."
            //},
            //new ActionsModel()
            //{
            //    Name = "Straight locomotion",
            //    Description = "With your left controller, push the thumbstick forward to glide forward (Thumbstick is the tallest button on the top of the controller and can be pushed around in a 360 degree movement), backwards to glide back, and sideways to move either to the left or right.  "
            //},
            //new ActionsModel()
            //{
            //    Name = "Rotating Around",
            //    Description = "With your right controller, push the thumbstick to the right or left to turn accordingly. Note: To avoid nausea, we provide snap turning."
            //},
            //new ActionsModel()
            //{
            //    Name = "Opening and closing Doors",
            //    Description = "Point the controller to the door knob, the ray from the controller will turn orange once it detects the knob, at that moment, on the left controller,  hold the grip button (This is the big button on the inner side of the controller where the folding fingers rest.) move your hand as if you are pulling or pushing the door."
            //},
            //new ActionsModel()
            //{
            //    Name = "Clicking buttons and switches",
            //    Description = "Point the controller to the button or switch of interest, once the ray from the controller turns orange, press the grip button once."
            //},
            //new ActionsModel()
            //{
            //    Name = "Final Conclusion",
            //    Description = "Thanks for completing the introduction tutorial, now lets proceed with the next tutorial."
            //},

            //new ActionsModel()
            //{
            //    Name="openShelterDoorText",
            //    Description = "To open the Localizer door, come closer and grab the door handle with your left hand, then pull it."
            //},

            //new ActionsModel()
            //{
            //    Name = "manageShelterLights",
            //    Description = "Once you've entered, on your left, there is a switch, press it to turn on the lights, and again to turn off."
            //},
            //new ActionsModel()
            //{
            //    Name="openLZZFrontDoor",
            //    Description = "Now face the front of the localizer and grab the front door handle and pull to open the door."
            //}
            
        //};
    }
}
