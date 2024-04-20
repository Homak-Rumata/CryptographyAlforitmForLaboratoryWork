using ENCODER.DecriptingAnalyzer;
using ENCODER.NumAlgoritm;
using ENCODER.FestelNet;
using ENCODER.AsymetrikEncoder;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;

namespace ENCODER
{
    internal class Program
    {

        static int GetNum(char value)
        {
            const int startvalue = 1040;

            if (((int)value >= 1040 & (int)value <= (1040+37)) || (int)value == 1025 || (int)value == 32 )
            {
                int place = (int)value switch
                {
                    >=1040 and <1046 => (int)value - startvalue,
                    1025 => 6,
                    32 => 34,
                    _ => ((int)value + 1 - startvalue)

                };
                return place;
            }
            else return (int)value;
        }
        static char GetSymbol(int num)
        {
            
            const int startvalue = 1040;
            char result = (num) switch
            {
                <=5 => (char)(num+startvalue),
                (6) => (char)(1025),
                34 => (char)(32),
                _ => (char) (num + startvalue - 1)
            };
            return result;
        }

        delegate R FG<T, R>(R item);
        delegate R FG<T1, T2, R>(T1 h, T2 B);
        

        //a-1072
        //A-1040

        //я-1103
        //Я-1071
        static void Main(string[] args)
        {

            RSA rsa = new RSA(131, 191);

            var word = new int[] { 2, 3, 4, 5, 6, 7 };

            var intConcatFunction = (IEnumerable<int> i) => (i.Aggregate("",(i1, i2)=>(i1+i2+", ")));

            var intintConcatFunction = (IEnumerable<(int a, int b)> i) => (i.Aggregate("\n\t", (i1, i2) => (i1+ "a: "+i2.a + " b: "+i2.b + ", \n\t")));

            var boolConcatFunction = (IEnumerable<bool> i) => (i.Aggregate("", (i1, i2) => (i1+(i2?"1":"0"))));

            var restest = rsa.Code(word.ToArray());

            var res1test = rsa.UnCode(restest).ToArray();

            var rsaKeys = rsa.GetKey();

            Console.WriteLine("\nRSA\n"+$"\tP = 131, Q = 191\n key: \n\te = {rsaKeys.e}, n = {rsaKeys.n} \n OriginalWord: \t{intConcatFunction(word)}\n CodedWord: \t{intConcatFunction(restest)}\n DeCodedWord: \t{intConcatFunction(res1test)}\n");

            ElGamal elGamal = new ElGamal(131, 191);

            var elGamaltest = elGamal.Code(word, 48).ToArray();

            var elGamal1test = elGamal.UnCode(elGamaltest).ToArray();

            var elGamalKeys = elGamal.GetKey();

            Console.WriteLine("\nАлгоритм Эль Гамаля\n" + $"\tP = 131, Q = 191\n key: \n\ty = {elGamalKeys.y}, g = {elGamalKeys.g}, y = {elGamalKeys.y} \n OriginalWord: \t{intConcatFunction(word)}\n CodedWord: \t{intintConcatFunction(elGamaltest)}\n DeCodedWord: \t{intConcatFunction(elGamal1test)}\n");

            var BackPack = new BackPack(new int[]{ 2, 7, 11, 21, 42, 89, 180, 354 }, 881);

            var BoolArray = new bool[] { false, true, true, false, false, false, false, true };

            var BackPackTest = BackPack.Code(BoolArray);

            var Back1PackTest = BackPack.UnCode(BackPackTest).ToArray();

            var BackPackKeys = BackPack.GetKey();

            Console.WriteLine("\nРанцевый шифр\n" + $"\tOriganalKey = {intConcatFunction(new int[] { 2, 7, 11, 21, 42, 89, 180, 354 })}\n \tOpenkey: {intConcatFunction(BackPackKeys)} \n \tOriginalWord: {boolConcatFunction(BoolArray)}\n \tCodedWord: \t{BackPackTest}\n\t DeCodedWord: \t{boolConcatFunction(Back1PackTest)}\n");

            var ghdr = Pollada.GetValue(4567897);

            IOperationBlock<int> ioperation = new FestelOperationBlock<int>(new byte[] { 11, 6, 5, 1, 1, 10, 8, 1 }, (x) => (x));

            IFestelRound<int> round = new FestelRound<int>((x => (x)), ioperation);


            IEnumerable<IFestelRound<int>> glob = (new int[32]).Select(x => round).ToArray();

            DES<int> dES = new DES<int>(glob, 1, (int x)=>(x));

            var tempestiu = dES.OpenValueStream(new int[] { 1, 2, 2, 1, 45, 11, 2, 6}).ToArray();

            Console.WriteLine(tempestiu);

            var resultdes = dES.CodedValueStream(tempestiu).ToArray();

            SpecialInt vah = new SpecialInt(32,91);

            var gtemp = vah * vah;

            for (int i = 1040; i < (1040 + 33 + 33); i++)
            {
                Console.WriteLine($"{i} - {(char)i}");
            }

           var c = StatisticTools.ConvertToResultDictionary(StatisticTools.GetElemnts("Репников Никита Иванович", 2).Select(x=> KeyValuePair.Create(x.Key.ToCharArray(), x.Value)).ToDictionary(), (char[] a)=>(new string(a)));

            var g = StatisticTools.GetElemnts("Репников Никита Иванович".ToCharArray(), 2).Select(x => new KeyValuePair<string, int>(new string(x.Key), x.Value)).ToArray();


            int size = 5;
            int[,] ar = new int[,] {{ 2, 1, 1} , { 1, 2, 1}, { 1, 1, 2}};
            int[,] ar2 = new int[,] { { 3,4}, {3,5 }};

            Matrix m = new Matrix(ar, 37).Trans();
            Matrix b = new Matrix(ar2, 1000000);
            Console.WriteLine((m).Printf().Determination().GetNum + "\n____________\n");
            //Console.WriteLine((b*m).Printf().Determination().GetNum + "\n____________\n");

            IShifr shifrator = new HillShifr("цтще".ToUpper(), GetNum, GetSymbol, 29, 2);

            IShifr polyShifrator = new PolyHillShifr("вбббвбббв".ToUpper(), "вбббвбббв".ToUpper(), GetNum, GetSymbol, 37, 3);

            IShifr viginerShifr = new Viginer(GetNum, GetSymbol, "ввбг", 37);

            Console.WriteLine(shifrator.Code("хкчй".ToUpper()));
            Console.WriteLine(shifrator.UnCode(shifrator.Code("Репников Никита Иванович".ToUpper())));

            Console.WriteLine(polyShifrator.Code("Репников Никита Иванович".ToUpper()));
            Console.WriteLine(polyShifrator.UnCode(polyShifrator.Code("Репников Никита Иванович".ToUpper())));

            Console.WriteLine(viginerShifr.Code("Репников Никита Иванович".ToUpper()));
            Console.WriteLine(viginerShifr.UnCode(viginerShifr.Code("Репников Никита Иванович".ToUpper())));


            Console.WriteLine(ExpressionEvklidAlgoritm.GetReverse(27, 37));

            string Message = Console.ReadLine();
            Func<char, char> Shifr = AfinCoder.Shifr(26, 20);
            Func<char, char> DeShifr = AfinCoder.UnShifr(26, 20);
            Message = new string(Message.ToUpper().ToCharArray().Select(x => Shifr(x)).ToArray());
            Console.WriteLine(Message);
            Message = new string(Message.ToUpper().ToCharArray().Select(x => DeShifr(x)).ToArray());
            Console.WriteLine(Message);
            Func<char, char> ShifrExp = AfinCoderExperimental.Shifr(26, 20);
            Func<char, char> DeShifrExp = AfinCoderExperimental.Unshifr(26, 20);
            Message = new string(Message.ToUpper().ToCharArray().Select(x => ShifrExp(x)).ToArray());
            Console.WriteLine(Message);
            Message = new string(Message.ToUpper().ToCharArray().Select(x => DeShifrExp(x)).ToArray());
            Console.WriteLine(Message);



            Console.ReadLine();

            //GetToken();

        }

        /*static async Task<string> GetToken()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://egrul.nalog.ru");
            StringContent cn = new StringContent ("vyp3CaptchaToken=&page=&query=%D0%A2%D0%A3%D0%A1%D0%A3%D0%A0&region=&PreventChromeAutocomplete=");
            var result = await httpClient.PostAsync(httpClient.BaseAddress, cn);
            string content = await result.Content.ReadAsStringAsync();

            Console.WriteLine(content);
            return content;
        }*/

        class Based
        {
            Content[] rows;
        }

        class Content
        {
            public string a;
            public string c;
            public string g;
            public string cnt;
            public string i;
            public string k;
            public string n;
            public string o;
            public string p;
            public string t;
            public string tot;
            public string o184;
        }

    }
}
