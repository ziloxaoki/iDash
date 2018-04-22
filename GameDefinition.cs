using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iDash
{
    public enum GameEnum
    {
        RACE_ROOM, PCARS_64BIT, PCARS_32BIT, PCARS_NETWORK, RF1, RF2, ASSETTO_64BIT, ASSETTO_32BIT, IRACING_64BIT, F1_CODEMASTER
    }
    public class GameDefinition
    {
        public static GameDefinition pCars64Bit = new GameDefinition(GameEnum.PCARS_64BIT, "PCars 64 bits", "pCARS64",
            "pcars64_launch_exe", "pcars64_launch_params", "launch_pcars");
        public static GameDefinition pCars32Bit = new GameDefinition(GameEnum.PCARS_32BIT, "Pcars 32 bits", "pCARS",
            "pcars32_launch_exe", "pcars32_launch_params", "launch_pcars");
        public static GameDefinition raceRoom = new GameDefinition(GameEnum.RACE_ROOM, "Raceroom", "RRRE",
            "r3e_launch_exe", "r3e_launch_params", "launch_raceroom");
        public static GameDefinition iracing = new GameDefinition(GameEnum.IRACING_64BIT, "IRacing 64 bits", "iRacingSim64DX11",
            "iracing_launch_exe", "iracing_launch_params", "launch_iracing");
        public static GameDefinition rFactor1 = new GameDefinition(GameEnum.RF1, "RFactor", "rFactor",
            "rf1_launch_exe", "rf1_launch_params", "launch_rfactor1");
        public static GameDefinition gameStockCar = new GameDefinition(GameEnum.RF1, "Game Stock Car", "GSC",
            "gsc_launch_exe", "gsc_launch_params", "launch_gsc");
        public static GameDefinition automobilista = new GameDefinition(GameEnum.RF1, "Automobilista", "AMS",
            "ams_launch_exe", "ams_launch_params", "launch_ams");
        public static GameDefinition marcas = new GameDefinition(GameEnum.RF1, "Marcas", "MARCAS",
            "marcas_launch_exe", "marcas_launch_params", "launch_marcas");
        public static GameDefinition ftruck = new GameDefinition(GameEnum.RF1, "Formula Truck", "FTRUCK",
            "ftruck_launch_exe", "ftruck_launch_params", "launch_ftruck");
        public static GameDefinition rfactor2 = new GameDefinition(GameEnum.RF2, "RFactor 2", "RFACTOR2",
            "rfactor2_exe", "rfactor2_params", "rfactor2");
        public static GameDefinition assetto64Bit = new GameDefinition(GameEnum.ASSETTO_64BIT, "Assetto 64 bits", "acs",
            "acs_launch_exe", "acs_launch_params", "launch_acs");
        public static GameDefinition assetto32Bit = new GameDefinition(GameEnum.ASSETTO_32BIT, "assetto_32_bit", "acs_x86",
            "acs_launch_exe", "acs_launch_params", "launch_acs");
        public static GameDefinition f1Codemaster = new GameDefinition(GameEnum.F1_CODEMASTER, "F1 Codemaster", "F1_2017",
            "F1_2017_exe", "f1_launch_params", "launch_f1"); 



        public static List<GameDefinition> getAllGameDefinitions()
        {
            List<GameDefinition> definitions = new List<GameDefinition>();
            definitions.Add(automobilista); definitions.Add(gameStockCar); definitions.Add(marcas);
            definitions.Add(ftruck); definitions.Add(rFactor1); definitions.Add(assetto32Bit); definitions.Add(assetto64Bit);
            definitions.Add(raceRoom); definitions.Add(rfactor2); definitions.Add(iracing); definitions.Add(f1Codemaster);
            return definitions;
        }

        public static GameDefinition getGameDefinitionForFriendlyName(String friendlyName)
        {
            List<GameDefinition> definitions = getAllGameDefinitions();
            foreach (GameDefinition def in definitions)
            {
                if (def.friendlyName == friendlyName)
                {
                    return def;
                }
            }
            return null;
        }

        public static GameDefinition getGameDefinitionForEnumName(String enumName)
        {
            List<GameDefinition> definitions = getAllGameDefinitions();
            foreach (GameDefinition def in definitions)
            {
                if (def.gameEnum.ToString() == enumName)
                {
                    return def;
                }
            }
            return null;
        }

        public static String[] getGameDefinitionFriendlyNames()
        {
            List<String> names = new List<String>();
            foreach (GameDefinition def in getAllGameDefinitions())
            {
                names.Add(def.friendlyName);
            }
            return names.ToArray();
        }

        public GameEnum gameEnum;
        public String friendlyName;
        public String processName;
        public String gameStartCommandProperty;
        public String gameStartCommandOptionsProperty;
        public String gameStartEnabledProperty;

        public GameDefinition(GameEnum gameEnum, String friendlyName, String processName, 
            String gameStartCommandProperty, String gameStartCommandOptionsProperty, String gameStartEnabledProperty)
        {
            this.gameEnum = gameEnum;
            this.friendlyName = friendlyName;
            this.processName = processName;            
            this.gameStartCommandProperty = gameStartCommandProperty;
            this.gameStartCommandOptionsProperty = gameStartCommandOptionsProperty;
            this.gameStartEnabledProperty = gameStartEnabledProperty;
        }
    }
}
