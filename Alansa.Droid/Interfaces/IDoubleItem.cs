namespace Alansa.Droid.Interfaces
{
    /// <summary>
    /// For use in the sub item list with datum like income source, expense types etc.
    /// </summary>
    internal interface IDoubleItem
    {
        string GetPrimaryText();

        string GetSecondaryText();
    }
}