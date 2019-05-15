using UnityEngine;

/// <summary>
/// Script to Call Requests to delete or logout current user.
/// </summary>
public class DeleteUser : MonoBehaviour
{
    /// <summary>
    /// Calls Request to Delete user from database.
    /// </summary>
    public void CallDeleteUser()
    {
        UserController.GetInstance().CallDeleteUser();
    }

    /// <summary>
    /// Run Logout from UserController.
    /// </summary>
    public void CallLogOut()
    {
        UserController.GetInstance().LogOut();
    }
}
