using System.Collections.Generic;
using Utils;

namespace Control
{
    public static class MusicalInstruments
    {
        private static List<string> instruments = new()
        {
            "Aulos",
            "Bendir",
            "Bouzouki",
            "DoubleBass",
            "Dulcimer",
            "Harp",
            "Karamuza",
            "Toubelei",
        };

        public static List<string> Instruments() => instruments;

        public static string GetRandomInstrument() => RandomHelper<string>.GetRandomFromList(instruments);
    }
}