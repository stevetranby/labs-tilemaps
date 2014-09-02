using System.Collections;
using System.Collections.Generic;

namespace ST
{
    /// <summary>
    /// Simulation config for a game like SimCity or GameDevStory
    /// 
    /// TODO:
    /// - what goes here instead of game state?
    /// - rates? (pollution/crime/fire/value/population/etc)
    /// </summary>
    public class SimulationConfig
    {
        int elapsedTimeUnits;
        int timeUnitsPerYear;
        Dictionary<string,string> propertyBag;

        // calculate next iteration
        public void Step() {}
    }
}