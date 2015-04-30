using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]
    public enum ToleranceUnits
    {
        /// <summary>
        /// millimass units: a unit of mass equal to 0.001 atomic mass unit, used in physics 
        /// and chemistry. This unit is also called the millidalton (mDa). The millimass unit 
        /// is an SI unit, but its proper SI symbol is mu, not the older symbol mmu
        /// </summary>
        [EnumMember]
        mmu,
        /// <summary>
        /// parts-per-million: a concentration, a unit of proportion equal to 10^-6.
        /// </summary>
        [EnumMember]
        ppm,
        /// <summary>
        /// atomic mass units: the unit of mass used by chemists and physicists for measuring 
        /// the masses of atoms and molecules. Early in the nineteenth century, scientists 
        /// discovered that each chemical element is composed of atoms, and that each chemical 
        /// compound is composed of molecules in which atoms are combined in a fixed way. The 
        /// definition of the unified atomic mass unit as 1/12 the mass of the most common atoms
        /// of carbon, known as carbon-12 atoms. (Most elements are mixtures of atoms which have
        /// different masses because they contain different numbers of neutrons; these varieties
        /// are called isotopes.) Careful experiments have measured the size of this unit; the 
        /// currently accepted value (1998) is 1.660 538 73 x 10-24 grams. (This number equals 1 
        /// divided by Avogadro's number; see mole.) In addition, 1 amu equals approximately 
        /// 931.494 MeV (see electron volt). In biochemistry, the atomic mass unit is called 
        /// the dalton (Da). 
        /// </summary>
        [EnumMember]
        amu
    }

}
