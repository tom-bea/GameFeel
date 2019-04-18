Hey Jireh, I'll try to keep this README brief.

Input Settings:
I use the Horizontal and Vertical axis.
I use the Fire1 button.
I use Restart, Next, Previous, and Start buttons which are new buttons I created.

Controls:
Movement: axis are bound to WASD
Firing: Fire1 is bound to Left Mouse Click
Restarting: Restart button is bound to \
Nexting: Next button is bound to ]
Previousing: Previous button is bound to [
Starting: Start button is bound to p


Bugs:
The rotation is broken-ish. Basically it goes from 170 to -170 so rather than rotating 20 degress counter-clockwise, it rotates 340 degress clockwise.



Process for adding new slides:
1. Open up GameManager and the script that will be used to make the gameplay update
2. In GameManager, there is a region called "Delegates and Events", open it up.
3. Create a new event with an appropriate name for the update to be made. If you do NOT need to send any parameters, use EmptyDelegate. If you need to send an integer, use IntDelegate. If you need to send something else, make the appropriate delegate.
4. In GameManager, there is a region called "Between Slides Methods", open it up and find the PerformSlideAction method
5. Add in your new event with its corresponding slide number (probably the next number)
6. In the script that will be used to make the actual update, create a method that will be called in order to perform the update
7. Add the following in that same script:
#region OnEnable and OnDisable
private void OnEnable()
{
   GameManager.NameOfEvent += NameOfMethodToCall;
}

private void OnDisable()
{
   GameManager.NameOfEvent -= NameOfMethodToCall;
}
#endregion
8. Test!