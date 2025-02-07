namespace SMEConnect.Constatns
{
    public class Constants
    {
        public static class ApiResponseType
        {
            public const string Success = "Success";

            public const string Failure = "Error";

            public const string NotFound = "NotFound";
        }

        public enum DiscussionStatus
        {
            Open,
            Closed,
            Star
        }

        public static string[] Claims = { "DeleteTask", "ViewReport", "ManageSystem" };

        public static class ApiErrors
        {
            public static string ErrorRetrivingEmployees = "Error retrieving employees: ";
            public static string NullEmployee = "Employee data is null.";
            public static string ErrorCreatingEmployee = "Error creating employee: ";
            public static string ErrorUpdatingEmployee = "Error updating employee: ";
            public static string ErrorDeletingEmployee = "Error deleting employee: ";
            public static string DuplicateEmailOrUser = "Duplicate email id, User already exists!";
            public static string ErrorCreatingUser = "Error whiel creating user.";
            public static string ErrorRetrivingTasks = "Error retrieving tasks: ";
            public static string NullTask = "Task data is null.";
            public static string ErrorCreatingTask = "Error creating task: ";
            public static string ErrorUpdatingTask = "Error updating task: ";
            public static string ErrorDeletingTask = "Error deleting task: ";
            public static string TaskNotFound = "Task not found.";
            public static string UserCreated = "User created successfully.";

        }

        public static class ModuleName
        {
            public static string Employee = "Employee";
            public static string TaskItem = "Task";
        }

        public static class RoleName
        {
            public static string Admin = "admin";
            public static string Member = "member";
            public static string Aseme = "aseme";
            public static string Director = "director";
        }

        public static class AccessConfigurationErrorMessage
        {
            public static string RoleExist = "Role already exist.";
            public static string ErrorWhileCreatingRole = "Error while creating new role.";
            public static string UserNotFound = "User not found.";
            public static string FailedToAddRoleToUser = "Failed to add role to user.";
            public static string FailedToAddClaimToUser = "Failed to add claim to user.";
            public static string ErrorWhileAddingClaim = "Error while adding claim";
            public static string RoleNotFound = "Role not found";
            public static string AddedClaimToRole = "Added claim to role.";
            public static string WrongEmailPassword = "Wrong email or passord";
            public static string FaildToLogout = "Faild to logout";
            public static string FaildToUpdate = "Faild to update.";
            public static string FaildToResetPassword = "Failed to reset password";
            public static string OldPasswordIncorrect = "Old password is incorrect";
            public static string QuestionOrAnswerIsWrong = "Question or answer is wrong.";
            public static string FaildToUpdateUser = "Failed to update user details.";
            public static string UserAlreadyExist = "User already exists.";
            public static string UserCreationFaild = "User creation failed! Please check user details and try again.";

        }

        public static class AccessConfigurationSccessMessage
        {
            public static string NewRoleAdded = "New role added successfully.";
            public static string AddedRoleToUser = "Added role to user successfully.";
            public static string ClaimAddedToUser = "Claim added to user.";
            public static string UserCreated = "User created successfully.";
            public static string UserLoggedOut = "User logged out successfully.";
            public static string UpdatedUser = "Updated user.";
            public static string PasswordUserUpdated = "Password and user details updated successfully";
            public static string FaliedToUpdate = "Failed to update user details";
            public static string AddedNewGroup = "New group added.";
        }

        public static class Query
        {
            public static string selectUsers = "SELECT * FROM AspNetUsers WHERE Email = @email";
        }
    }
}
