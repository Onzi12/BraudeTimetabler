namespace Api
{
    public static class HelperMethods
    {
        /// <summary>
        /// stupid helper method, to make lazy read only prop easier
        /// </summary>
        /// <typeparam name="T">type of the field</typeparam>
        /// <param name="field">the field</param>
        /// <returns>the create field</returns>
        public static T GetOrCreate<T>(ref T field) where T : class, new()
        {
            return field ?? (field = new T());
        }
    }
}