
namespace DirectOutput
{
    /// <summary>
    /// Enum for the different TableElement types.
    /// </summary>
    public enum TableElementTypeEnum
    {
        /// <summary>
        /// Unknown table element.
        /// </summary>
        Unknown='?',
        /// <summary>
        /// Lamp.
        /// </summary>
        Lamp='L',
        /// <summary>
        /// Switch.
        /// </summary>
        Switch='W',
        /// <summary>
        /// Solenoid.
        /// </summary>
        Solenoid='S',
        /// <summary>
        /// General illumination.
        /// </summary>
        GIString='G',
        /// <summary>
        /// Mech object.
        /// </summary>
        Mech='M',
        /// <summary>
        /// Mech object value from GetMech (PinMame/B2SServer specific)
        /// </summary>
        GetMech='N',
        /// <summary>
        /// EM table element.
        /// </summary>
        EMTable='E',
        /// <summary>
        /// LED
        /// </summary>
        LED='D',
        /// <summary>
        /// Score
        /// </summary>
        Score='C',
        /// <summary>
        /// Score digit
        /// </summary>
        ScoreDigit='B',
        /// <summary>
        /// Named table element
        /// </summary>
        NamedElement='$'


    }
}
