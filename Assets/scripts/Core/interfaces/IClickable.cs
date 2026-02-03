namespace localizer.core.interfaces
{
	/// <summary>
	/// this interface is used by the raycast system of the camera. it must be inherited by all the gameobject scripts 
	/// which are to be clicked and detected by  the game.
	/// </summary>
	public interface IClickable
	{
		void ClickAction();
	}

}
