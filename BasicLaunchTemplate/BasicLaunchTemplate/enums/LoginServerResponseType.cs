namespace BasicLaunchTemplate
{
    public enum LoginServerResponseType : byte
    {
        UserLoginCorrect,
        CypherError,
        CannotFindNamePwCombination,
        UserAlreadyExists,
        UserSuccessfullyCreated,
        UserBanned,
        RateLimit
    }
}
