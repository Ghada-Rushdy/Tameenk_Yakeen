using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.Yakeen.DAL.Enums
{
   public enum MedicalCondition
    {
        NoRestriction = 1,
        /// <summary>
        /// Automatic Car.
        /// </summary>
       
        AutomaticCar = 2,
        /// <summary>
        /// Prosthetic Limb.
        /// </summary>
       
        ProstheticLimb = 3,
        /// <summary>
        /// Vision Augmenting Lenses.
        /// </summary>
        
        VisionAugmentingLenses = 4,
        /// <summary>
        /// Only Day Time.
        /// </summary>
      
        OnlyDayTime = 5,
        /// <summary>
        /// Hearing Aid
        /// </summary>
        
        HearingAid = 6,
        /// <summary>
        /// Driving Inside KSA Only.
        /// </summary>
        DrivingInsideKSAOnly = 7,
        /// <summary>
        /// Handicap Car.
        /// </summary>
        HandicapCar = 8,
        /// <summary>
        /// For Private Use With No Payment.
        /// </summary>
        ForPrivateUseWithNoPayment = 9
    }
}
