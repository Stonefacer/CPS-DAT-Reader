using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_Reader.FileReaders {
    class StopsFactory {

        public const int DataMask = 0x0000ffff;
        public const int SeparatorFlag = 0x08000000;
        public const int UserMIDIFlag       = 0x04000000;
        public const int UserMIDIFlagB = 0x01000000;
        public const int UserMIDIFlags = UserMIDIFlag | UserMIDIPGMFlag | UserMIDIFlagB;
        public const int UserMIDIPGMFlag = 0x02000000;

        private static Dictionary<int, string[]> CPTable = new Dictionary<int, string[]>()
        {
            {1, new string[3]{"Swell to Pedal 8", "Great to Pedal 8", "?1cp3?"}},
            {2, new string[3]{"Swell to Great 8", "?2cp2?", "?2cp3?"}},
            {4, new string[3]{"MEL Swell", "BASS", "?4cp3"}}
        };

        private static Dictionary<int, string[]> GreatTable = new Dictionary<int, string[]>()
        {
            {1, new string[4]{"16 Bordon", "16 Violone", "16 Principal", "16 Quintaton"}},
            {2, new string[4]{"8 Principal", "8 OpenDiap", "8 Montre", "8 Prinzipal"}},
            {3, new string[4]{"8 Gemshorn", "8 2ndDiapson", "8 VoceUmanII", "8 Aeoline"}},
            {4, new string[4]{"8 Gedackt", "8 Claribella", "8 Flute Harm", "8 Bourdon"}},
            {5, new string[4]{"4 Octava", "4 Principal", "4 Flute aBec", "4 Fugara"}},
            {6, new string[4]{"?6gt1?", "?6gt2?", "?6gt3?", "?6gt4?"}},
            {7, new string[4]{"2-2/3  Quinte", "2-2/3 Twlfth", "2-2/3 Nazard", "8 Quintaden"}},
            {8, new string[4]{"2 Super Oct", "2 Fifteenth", "2 Doublette", "2 Piccolo"}},
            {9, new string[4]{"IV Mixtur", "V TierceMix", "V Cornet", "IV Rauschqu"}},
            {10, new string[4]{"?10gt1?", "?10gt2?", "?10gt3?", "?10gt4?"}},
            {11, new string[4]{"8 Trompete", "8 Waldhorn", "8 Trompette", "8 Cromorne"}},
            {13, new string[4]{"Tremulant", "?13gt2?", "?13gt3?", "?13gt4?"}}
        };

        private static Dictionary<int, string[]> PedalTable = new Dictionary<int, string[]>()
        {
            {1, new string[4]{"32 C Viol", "32 D O Diap", "32 Prestant", "32 Bourdon"}},
            {2, new string[4]{"16 Principal", "16 OpenWood ", "16 Contrabass", "16 Violone"}},
            {3, new string[4]{"16 Subbass ", "16 2nd Subbass", "16 Bourdon", "16 Quintaton"}},
            {4, new string[4]{"16 Brdn Dou", "16 Lieb Ged", "16 Pommer", "16 Bourdon"}},
            {5, new string[4]{"8 Octava", "8 Oktave", "8 Montre ", "8 Violncello"}},
            {6, new string[4]{"8 Bourdon ", "8 StoppedFlute", "8 Flute ", "8 Still Gedackt"}},
            {7, new string[4]{"4 Choralbass", "4 Fifteenth", "4 Flute de bois", "10 2/3 Quintflote"}},
            {8, new string[4]{"IV Mixture", "10-2/3 Quint", "IV Mixture", "IV Fourniture"}},
            {9, new string[4]{"32 C Bomba", "32 Ophecleid", "32 Bazuin", "32 C Basson"}},
            {10, new string[4]{"16 Posaune", "16 Trombone", "16 Bombarde", "16 Basson"}},
            {11, new string[4]{"8 Trompete", "8 Trumpet ", "8 Trompette ", "8 Dulzian"}},
            {12, new string[4]{"4 Klarine", "4 Cornet Clarion", "4 Clairon", "4 Schalmei"}}
        };

        private static Dictionary<int, string[]> SwellTable = new Dictionary<int, string[]>()
        {
            {1, new string[4]{"8 StlGedackt", "8 StopDiap ", "8 Bourdon ", "8 RorGedackt"}},
            {2, new string[4]{"8 SpitzGeign", "8 GeigenDiap", "8 Salicional", "8 Viola"}},
            {3, new string[4]{"8 ViolaCel II", "8 FluteCel II", "8 VoixCel II", "8 Schwbung II"}},
            {4, new string[4]{"4 Principal", "4 UndMaris II", "4 Prestant", "4 MutdViolII"}},
            {5, new string[4]{"4 Nachthorn", "4 Solo Flute", "4 FlTravers", "4 SpillFlote"}},
            {6, new string[4]{"2 2/3 Nasat", "2 2/3 Twlfth", "2 2/3 Nazard", "2 2/3 Quinte"}},
            {7, new string[4]{"2 Piccolo", "2 Waldflote", "2 Doublette", "2 Schwegel"}},
            {8, new string[4]{"II Sesquialt", "II DolcCorne", "1 3/5 Tierce", "1 1/3 Larigt"}},
            {9, new string[4]{"III Mixture", "III Cymbale", "IV Plein Jeu", "1 Sifflote"}},
            {10, new string[4]{"?13sw1?", "?13sw2?", "?13sw3?", "?13sw4?"}},
            {11, new string[4]{"8 Schalmei", "8 Oboe", "8 Hautbois", "8 Trompette"}},
            {13, new string[4]{"Tremulant", "?13sw2?", "?13sw3?", "?13sw4?"}}
        };

        private static Dictionary<int, string[]> TableMIDIUserVoices = new Dictionary<int, string[]>(){
            {1, new string[3]{"PRINCIPAL 32’", "Principal", "1"}},
            {2, new string[3]{"CONTRA GEIGEN 32’", "String", "2"}},
            {3, new string[3]{"CONTRA BOURDON 32’", "Flute", "3"}},
            {7, new string[3]{"SPITZ PRINZIPAL 16’", "Principal", "4"}},
            {8, new string[3]{"LIEBLICH GEDACKT 16’", "Flute", "5"}},
            {9, new string[3]{"DULCIANA 16’", "String", "6"}},
            {10, new string[3]{"CELLOS CÉLESTES II 16’", "Celeste", "7"}},
            {11, new string[3]{"ERZHALER CÉLESTES II 16’", "Celeste", "8"}},
            {15, new string[3]{"MONTRE 8’", "Principal", "9"}},
            {16, new string[3]{"BACH PRINCIPAL 8’", "Principal", "10"}},
            {17, new string[3]{"GEMSHORN 8’", "Hybrid", "11"}},
            {18, new string[3]{"GAMBA 8’", "String", "12"}},
            {19, new string[3]{"DULCIANA 8’", "String", "13"}},
            {20, new string[3]{"HUZGEDACKT 8’", "Flute", "14"}},
            {21, new string[3]{"GROSS FLUTE 8’", "Flute", "15"}},
            {22, new string[3]{"2nd FLUTE HARMONIQUE 8’", "Flute", "16"}},
            {25, new string[3]{"CÉLESTES III 8’", "Celeste", "17"}},
            {26, new string[3]{"III UNDA MARIS III 8’", "Celeste", "18"}},
            {29, new string[3]{"PRINCIPAL 4’", "Principal", "19"}},
            {30, new string[3]{"OPEN FLUTE 4’", "Flute", "20"}},
            {31, new string[3]{"CHIMNEY FLUTE 4’", "Flute", "21"}},
            {34, new string[3]{"QUINTE 2-2/3’", "Flute", "22"}},
            {35, new string[3]{"NAZARD 2-2/3’", "Flute", "23"}},
            {36, new string[3]{"GEMSHORN 2’", "Hybrid", "24"}},
            {37, new string[3]{"PICCUO 2’", "Flute", "25"}},
            {41, new string[3]{"TIERCE 1-3/5’", "Flute", "26"}},
            {42, new string[3]{"LARIGOT 1-1/3’", "Flute", "27"}},
            {43, new string[3]{"SEPTIÈME 1-1/7’", "Flute", "28"}},
            {44, new string[3]{"SIFFLÖTE 1’", "Flute", "29"}},
            {48, new string[3]{"JEU DE CLOCHETTE II", "Principal", "30"}},
            {49, new string[3]{"QUARTANE II", "Principal", "31"}},
            {50, new string[3]{"32 II Scharf SCHARF II", "Principal", "32"}},
            {51, new string[3]{"GABLER CORNET V ", "Principal", "33"}},
            {52, new string[3]{"PEDAL GRAND MIXTUR VI", "Principal", "34"}},
            {53, new string[3]{"GRAVE MIXTUR IV", "Principal", "35"}},
            {54, new string[3]{"2nd GRAVE MIXTUR IV", "Principal", "36"}},
            {55, new string[3]{"FOURNITURE SS IV-VI", "Principal", "37"}},
            {56, new string[3]{"TIERCE FOURNITURE VI", "Principal", "38"}},
            {57, new string[3]{"GRAND MIXTUR VIII", "Principal", "39"}},
            {61, new string[3]{"DOUBLE OPHICLEIDE 32’", "Reed", "40"}},
            {62, new string[3]{"CONTRE BOMBARDE 32’", "Reed", "41"}},
            {63, new string[3]{"CONTRE BASSOON 32’", "Reed", "42"}},
            {64, new string[3]{"2nd BOMBARDE 16’", "Reed", "43"}},
            {65, new string[3]{"CONTRE TROMPETTE 16’", "Reed", "44"}},
            {66, new string[3]{"RANKETT 16’", "Reed", "45"}},
            {71, new string[3]{"TROMPET 8’", "Reed", "46"}},
            {72, new string[3]{"TROMPETTE 8’", "Reed", "47"}},
            {73, new string[3]{"TROMPETTE 8’", "Reed", "48"}},
            {74, new string[3]{"STATE TRUMPET 8’", "Reed", "49"}},
            {75, new string[3]{"CHAMADES 8’", "Reed", "50"}},
            {76, new string[3]{"DULZIAN 8’", "Reed", "51"}},
            {77, new string[3]{"CROMORNE 8’", "Reed", "52"}},
            {78, new string[3]{"BARYTON 8’", "Reed", "53"}},
            {79, new string[3]{"COR D’AMOUR 8’", "Reed", "54"}},
            {80, new string[3]{"REGAL 8’", "Reed", "55"}},
            {81, new string[3]{"VOX HUMANA 8’", "Reed", "56"}},
            {82, new string[3]{"VOX HUMAINE 8’ 8’", "Reed", "57"}},
            {83, new string[3]{"VOX HUMAINE T 8’", "Reed", "58"}},
            {87, new string[3]{"CLARION 4’", "Reed", "59"}},
            {88, new string[3]{"CLAIRON 4’", "Reed", "60"}},
            {89, new string[3]{"2nd CLAIRON 4’", "Reed", "61"}},
            {90, new string[3]{"ROHRSCHALMEI 4’", "Reed", "62"}},
            {94, new string[3]{"PRINCIPALS III 8+4+2", "Ensemble", "63"}},
            {95, new string[3]{"PRINCIPALS+MIXTURS 8+4+2+IV", "Ensemble", "64"}},
            {96, new string[3]{"FLUTE CÉLESTES IV 16+4", "Celeste", "65"}},
            {97, new string[3]{"CÉLESTES IV 16’+4’", "Célestes", "66"}},
            {98, new string[3]{"CÉLESTES VI 16+8+4", "Célestes", "67"}},
            {99, new string[3]{"CÉLESTES VII 16’+8’+4’+Vox", "Célestes", "68"}},
            {100, new string[3]{"VOXES II 16+8", "Reed", "69"}},
            {101, new string[3]{"VOXES II 16+4", "Reed", "70"}},
            {102, new string[3]{"FULL SWELL 16+8+4+Reeds", "Ensemble", "71"}},
            {105, new string[3]{"TIBIA 8’", "Flute", "72"}},
            {106, new string[3]{"TIBIA 4’", "Flute", "73"}},
            {107, new string[3]{"VDO CELESTE 8’", "Célestes", "74"}},
            {108, new string[3]{"VDO CELESTE T 8’", "Célestes", "75"}},
            {109, new string[3]{"TUBA T 8’", "Reed", "76"}},
            {110, new string[3]{"CORNOPEAN T 8’", "Reed", "77"}},
            {111, new string[3]{"TIBIAS 16’ + 8’+ VOX 16’ + 8’", "Theater", "78"}},
            {112, new string[3]{"FULL TIBIAS+STRS 8’+4’", "Theater", "79"}},
            {113, new string[3]{"VOX & STR 16’ + TIB 2’ + QUINT", "Theater", "80"}},
            {114, new string[3]{"VOX 16’+8’+CELESTES 8’", "Theater", "81"}},
            {115, new string[3]{"TIBIAS 8’, 4’ + STR", "Theater", "82"}},
            {116, new string[3]{"TIBIAS 8’, 4’ + STR + VOX", "Theater", "83"}},
            {117, new string[3]{"TIBIAS 8’, 4’ + TUBA 8’", "Theater", "84"}},
            {118, new string[3]{"TIBIA 4’ + CELESTES 8’", "Theater", "85"}},
            {119, new string[3]{"TIBIAS 4’, 2’ + STR 16’, 8’", "Theater", "86"}},
            {120, new string[3]{"TIBIA 4’ + KINURA 8’", "Theater", "87"}},
            {121, new string[3]{"STR 8’ + TIBIAS 2’ + GLOCKEN", "Theater", "88"}},
            {122, new string[3]{"TIBIA 4’ + GLOCKENSPIEL", "Theater\t\t", "89"}},
            {70, new string[3]{"ORCHESTRAL OBOE", "Orchestral", "94"}},
            {58, new string[3]{"ORCHESTRAL TRUMPET", "Orchestral\t", "97"}},
            {84, new string[3]{"HARPSICHORD LUTE", "Hybrid", "101"}},
            {85, new string[3]{"HARPSICHORD 8+8", "Hybrid", "102"}},
            {86, new string[3]{"HARPSICHORD 8+4", "Hybrid", "103"}}
        };

        private static Dictionary<int, string[]> GreatTableDAT = new Dictionary<int, string[]>()
        {
            {1, new string[4]{"Bourdon 16'", "Violone 16'", "Principal 16'", "Quintaton 16'"}},
            {2, new string[4]{"Principal 8'", "Open Diapason 8'", "Montre 8'", "Prinzipal 8'"}},
            {3, new string[4]{"Gemshorn 8'", "2nd Diapason 8'", "Voce Umana II 8'", "Aeoline 8'"}},
            {4, new string[4]{"Gedackt 8'", "Clarabella 8'", "Flûte Harmonique 8'", "Bourdon 8'"}},
            {5, new string[4]{"Octava 4'", "Principal 4'", "Flûte à Bec 4'", "Fugara 4'"}},
            {6, new string[4]{"Quinte 2-2/3'", "Twelfth 2-2/3'", "Nazard 2-2/3'", "Quintaden 8'"}},
            {7, new string[4]{"Superoctav 2'", "Fifteenth 2'", "Doublette 2'", "Piccolo 2'"}},
            {8, new string[4]{"Mixtur IV", "Tierce Mixture V", "Cornet V", "Rauschquint IV"}},
            {9, new string[4]{"Trumpet 8'", "Waldhorn 8'", "Trompette 8'", "Cromorne 8'"}}
        };

        private static Dictionary<int, string[]> SwellTableDAT = new Dictionary<int, string[]>()
        {
            {1, new string[4]{"Still Gedackt 8", "Stopped Diapason 8'", "Bourdon 8'", "Rohrgedackt 8'"}},
            {2, new string[4]{"Spitz Geigen 8'", "Geigen Diapason 8'", "Salicional 8'", "Viola 8'"}},
            {3, new string[4]{"Viola Céleste II 8'", "Flute Celeste II 8'", "Voix Céleste II 8'", "Schwebung II 8'"}},
            {4, new string[4]{"Principal 4'", "Unda Maris II 4'", "Prestant 4'", "Muted Viols II 4'"}},
            {5, new string[4]{"Nachthorn 4'", "Solo Flute 4'", "Flûte Traversière 4'", "Spillflöte 4'"}},
            {6, new string[4]{"Nasat 2 2/3'", "Twelfth 2-2/3'", "Nazard 2-2/3'", "Quinte 2-2/3'"}},
            {7, new string[4]{"Piccolo 2'", "Wald Flute 2'", "Doublette 2'", "Schwegel 2'"}},
            {8, new string[4]{"Sesquialtera II", "Dolce Cornet II", "Tierce 1-3/5'", "Larigot 1-1/3'"}},
            {9, new string[4]{"Mixture III", "Cymbale III", "Plein Jeu IV", "Sifflöte 1'"}},
            {10, new string[4]{"Schalmei 8'", "Oboe 8'", "Hautbois 8'", "Trompette 8'"}}

        };

        private static Dictionary<int, string[]> PedalTableDAT = new Dictionary<int, string[]>() {
            {1, new string[4]{"Principal 16'", "Open Wood 16'", "Contrabass 16'", "Violone 16'"}},
            {2, new string[4]{"Subbass 16'", "2nd Subbass 16'", "Bourdon 16'", "Quintaton 16'"}},
            {3, new string[4]{"Octava 8'", "Octave 8'", "Montre 8'", "Violoncello 8'"}},
            {4, new string[4]{"Bourdon 8'", "Stopped Flute 8'", "Flûte 8'", "Still Gedackt 8'"}},
            {5, new string[4]{"Choral Bass 4'", "Fifteenth 4'", "Flûte de Bois 4'", "Quintflöte 10-2/3'"}},
            {6, new string[4]{"Posuane 16'", "Trombone 16'", "Bombarde 16'", "Basson 16'"}},
            {7, new string[4]{"Trompete 8'", "Trumpet 8'", "Trompette 8'", "Dulzian 8'"}},
            {8, new string[4]{"Klarine 4'", "Cornet Clarion 4'", "Clairon 4'", "Schalmei 4'"}}
        };

        public static string StopTypeToString(StopType type) {
            switch(type) {
                case StopType.Great:
                case StopType.Swell:
                case StopType.Pedal:
                    return type.ToString();
                case StopType.CP:
                    return "CP";
                case StopType.GreatDAT:
                case StopType.PedalDAT:
                case StopType.SwellDAT:
                    return type.ToString().Substring(0, 5);
                default:
                    return "";
            }
        }

        public static Stop CreateMIDIStop(int PGM, int MSB, int LSB, StopType type, bool PlaceB = false) {
            int Flags = UserMIDIPGMFlag;
            if(PlaceB)
                Flags |= UserMIDIFlagB;
            return new StopMIDI(PGM, Flags, MSB, LSB, type);
        }

        public static Stop CreateSeparator(int M, int P) {
            return new Stop(((M - 1) * 100 + P - 1) & DataMask, SeparatorFlag, StopType.Ctrl);
        }

        public static string SeparatorToString(Stop separator) {
            return string.Format("M{0}P{1}", separator.id0 / 100 + 1, separator.id0 % 100 + 1);
        }

        public static string GetStopName(Stop st) {
            string[] res = null;
            switch(st.Type) {
                case StopType.Great:
                    if((st.id1 & StopsFactory.UserMIDIFlags) != 0)
                        return GetStopNameMIDI(st);
                    if(GreatTable.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    return string.Empty;
                case StopType.Swell:
                    if((st.id1 & StopsFactory.UserMIDIFlags) != 0)
                        return GetStopNameMIDI(st);
                    if(SwellTable.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                case StopType.Pedal:
                    if((st.id1 & StopsFactory.UserMIDIFlags) != 0)
                        return GetStopNameMIDI(st);
                    if(PedalTable.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                case StopType.CP:
                    if(CPTable.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                case StopType.GreatDAT:
                    if(GreatTableDAT.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                case StopType.SwellDAT:
                    if(SwellTableDAT.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                case StopType.PedalDAT:
                    if(PedalTableDAT.TryGetValue(st.id0 + 1, out res))
                        return res[st.id1];
                    else
                        return string.Empty;
                default:
                    return string.Empty;
            }
        }

        private static string GetStopNameMIDI(Stop st) {
            if((st.id1 & StopsFactory.UserMIDIPGMFlag) != 0) {
                int[] res = st.GetAdditionalInfo() as int[];
                return string.Format("MIDI User {3} PGM:{0} MSB:{1} LSB:{2}", st.id0, res[0], res[1], (st.id1&UserMIDIFlagB)==0?"A":"B");
            }
            else if((st.id1 & StopsFactory.UserMIDIFlag) != 0) {
                string[] res = null;
                if(TableMIDIUserVoices.TryGetValue(st.id0, out res))
                    return string.Format("{3}{0} {1} ({2})", res[2], res[0], res[1], (st.id1 & UserMIDIFlagB) == 0 ? "A" : "B");
            }
            return string.Empty;
        }
    }
}
