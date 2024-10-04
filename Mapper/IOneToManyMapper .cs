namespace Mapper
{
    /// <summary>  
    /// Defines mapping contract between objects.  
    /// Objects are identified by an integer identifier.  
    /// Valid identifiers are in range [1..947483647]   
    /// A parent may have multiple children.  
    /// A child may have only one parent.  
    /// </summary>  
    public interface IOneToManyMapper
    {
        /// <summary>  
        /// Defines a new mapping between parent and child  
        /// </summary>  
        /// <param name="parent">Parent identifier</param> 
        /// <param name="child">Child identifier</param> 
        void Add(int parent, int child);

        /// <summary>  
        /// Removes all mappings for a valid parent  
        /// </summary>  
        /// <param name="parent">Parent identifier</param> 
        void RemoveParent(int parent);

        /// <summary>  
        /// Removes a mapping for a valid child  
        /// </summary>  
        /// <param name="child">Child identifier</param>  
        void RemoveChild(int child);

        /// <summary>  
        /// Returns all (immediate) children for a given parent.  
        /// If there are no mappings for the parent, empty set is returned  
        /// </summary>  
        /// <param name="parent">Parent identifier</param>  
        /// <returns>Children identifiers</returns>  
        IEnumerable<int> GetChildren(int parent);

        /// <summary>  
        /// Returns a parent for a given child.  
        /// If there is no mapping for a child, returns 0  
        /// </summary>  
        /// <param name="child">Child identifier</param>  
        /// <returns>Parent identifier</returns>  
        int GetParent(int child);

        /// <summary>  
        /// Changes the valid object identifier for a given parent.  
        /// All mappings between old identifier should be migrated to the new identifier  
        /// </summary>  
        /// <param name="oldParent">Current parent identifier</param>  
        /// <param name="newParent">New parent identifier</param>  
        void UpdateParent(int oldParent, int newParent);

        /// <summary>  
        /// Changes the valid object identifier for a given child.  
        /// The parent for the child should be maintained.  
        /// </summary>  
        /// <param name="oldChild">Current child identifier</param>  
        /// <param name="newChild">New child identifier</param> 
        void UpdateChild(int oldChild, int newChild);
    }
}
