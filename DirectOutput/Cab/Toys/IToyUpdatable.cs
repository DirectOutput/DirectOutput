
namespace DirectOutput.Cab.Toys
{
    /// <summary>
    /// Interface for toys which need a update method which is used to update their outputs.
    /// </summary>
    public  interface IToyUpdatable: IToy
    {
        /// <summary>
        /// Toys implementing this method, should use it to tell the toy to update the assosiated outputs.<br/>
        /// Toys which are directly updating their outputs when their state changes dont need to implement this method and interface.
        /// </summary>
        void UpdateOutputs();
    }
}
