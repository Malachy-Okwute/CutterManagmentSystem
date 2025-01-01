namespace CutterManagement.Core
{
    /// <summary>
    /// Factory for <see cref="IDataAccessService{T}"/>
    /// </summary>
    public interface IDataAccessServiceFactory
    {

        /// <summary>
        /// Function to run whenever data changed in the database
        /// </summary>
        Func<object, bool> OnDataChanged { get; set; }

        /// <summary>
        /// Gets the desired table <see cref="IDataAccessService{T}"/> passed in as T
        /// </summary>
        /// <typeparam name="T">The type of table to get</typeparam>
        /// <returns><see cref="IDataAccessService{T}"/></returns>
        IDataAccessService<T> GetDbTable<T>() where T : class;
    }
}
