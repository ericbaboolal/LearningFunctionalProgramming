using System;
using Xunit;
using static Functional.F;
using Functional;

namespace Functional
{
    public static partial class OptionExt
    {
        // Implement Filter, Map, Bind

        // Filter :: Option<T> -> (T -> bool) -> Option<T>
        public static Option<T> Filter<T>(this Option<T> input, Func<T, bool> f) =>
            // This will help you start out
            // 1) Remove this line
            // None<T>()
            // 2) Uncomment the following and Fill in the blanks
            
            
            //the Some case is going to take the value inside of Some
            //the internal Some value is represented with t
            input.Match(
                Some: i => f(i) ? Some(i) : None<T>(),
                None: () => None<T>()
            )
            ;

        // Map :: Option<A> -> (A -> B) -> Option<B>
        public static Option<B> Map<A,B>(this Option<A> input, Func<A, B> f) =>
            // This will help you start out
            // 1) Remove this line
            // None<T>()
            // 2) Uncomment the following and Fill in the blanks
            
            
            //the Some case is going to take the value inside of Some
            //the internal Some value is represented with t
            //we want to convert the int value in T to a string
            input.Match(
                Some: t => Some<B>(f(t)),       
                None: () => None<B>()
            )
            ;

        // Bind :: Option<A> -> (A -> Option<B>) -> Option<B>
        public static Option<B> Bind<A,B>(this Option<A> input, Func<A, Option<B>> f ) =>


            input.Match(
                Some: t => f(t),
                None: () => None<B>()

            )
            ;
        public static Option<B> SelectMany<A,B>(this Option<A> input, Func<A, Option<B>> f ) =>


            input.Match(
                Some: t => f(t),
                None: () => None<B>()

            )
            ;

    }
}

namespace csharp
{
    public class OptionFilterTests
    {
        [Fact]
        public void Equality()
        {
            Assert.Equal(Some(1), Some(1));
            Assert.Equal(Some(Some(1)), Some(Some(1)));
            Assert.Equal(None<int>(), None<int>());

            Assert.NotEqual(Some(1), None<int>());
            Assert.NotEqual(Some(1), Some(2));
            Assert.NotEqual(Some(Some(1)), Some(Some(2)));
            Assert.NotEqual(Some(Some(1)), Some(None<int>()));

        }
        [Fact]
        public void Filter_Evens()
        {
            var result_evens = Some(2)
                            // Uncomment the following once you've implemented Filter
                            .Filter(i => i % 2 == 0)
                            ;
            Assert.Equal(Some(2), result_evens);
        }
        [Fact]
        public void Filter_Odds()
        {
            var result_odds = Some(2)
                            // Uncomment the following once you've implemented Filter
                            .Filter(i => i % 2 == 1)
                            ;
            Assert.Equal(None<int>(), result_odds);
        }
        [Fact]
        public void Filter_None()
        {
            var always_true = None<string>()
                            // Uncomment the following once you've implemented Filter
                            .Filter(i => true)
                            ;
            Assert.Equal(None<string>(), always_true);

            var always_false = None<string>()
                            // Uncomment the following once you've implemented Filter
                             .Filter(i => false)
                            ;
            Assert.Equal(None<string>(), always_false);
        }
    }
    public class OptionMapTests
    {
        [Fact]
        public void Map_Int_to_String()
        {
            var result = Some(1)
                            // Uncomment the following once you've implemented Map
                            .Map(i => $"Value: {i.ToString()}")
                            ;
            Assert.Equal(Some("Value: 1"), result);
        }
        [Fact]
        public void Map_None()
        {
            var result = None<string>()
                            // Uncomment the following once you've implemented Map
                            .Map(i => $"Value: {i}")
                            ;
            Assert.Equal(None<string>(), result);
        }
        [Fact]
        public void Map_Nested()
        {
            var result_even = Some(2)
                            // Uncomment the following once you've implemented Map
                            .Map(i => i % 2 == 0 ? Some(i.ToString()) : None<string>())
                            ;
            Assert.Equal(Some(Some("2")), result_even);

            var result_odd = Some(1)
                            // Uncomment the following once you've implemented Map
                            .Map(i => i % 2 == 0 ? Some(i.ToString()) : None<string>())
                            ;
            Assert.Equal(Some(None<string>()), result_odd);
        }
        [Fact]
        public void Map_Other()
        {
            var result_even = 
                Some("2")
                    .Map(i => OptionBindTests.ParseInt(i) )
                    ;
            Assert.Equal(Some(Some(2)), result_even);

            var r1 = 
                Some(2)
                    .Map(i => i + 3 );
            Assert.Equal(Some(5), r1);

             var r2 = 
                Some(2)
                    .Map(i => None<int>() );
            Assert.Equal(Some(None<int>()), r2);

            var r3 = 
                Some(2)
                    .Bind(i => Some(i + 3) );
            Assert.Equal(Some(5), r3);

        }
    }

    public class OptionBindTests
    {
        [Fact]
        public void Bind_Int_to_String()
        {
            var result = Some(1)
                            // Uncomment the following once you've implemented Map
                            .Bind(i => Some($"Value: {i}"))
                            ;
            Assert.Equal(Some("Value: 1"), result);
        }
        [Fact]
        public void Bind_None()
        {
            var result = None<string>()
                            // Uncomment the following once you've implemented Map
                            .Bind(i => Some($"Value: {i}"))
                            ;
            Assert.Equal(None<string>(), result);
        }
        [Fact]
        public void Bind_Nested()
        {
            var result_even = Some(2)
                            // Uncomment the following once you've implemented Map
                            .Bind(i => i % 2 == 0 ? Some(i.ToString()) : None<string>())
                            ;
            Assert.Equal(Some("2"), result_even);

            var result_odd = Some(1)
                            // Uncomment the following once you've implemented Map
                            .Bind(i => i % 2 == 0 ? Some(i.ToString()) : None<string>())
                            ;
            Assert.Equal(None<string>(), result_odd);
        }

        public static Option<int> ParseInt( string s){
            int result = 0;
            if (Int32.TryParse( s, out result)){
                return Some(result);
            } else{
                return None<int>();
            }
        }

        [Fact]
        public void Bind_Other()
        {
            var a = "1";
            var b = "2";
            var c = "four";
           
          //if valid int, just add to previous value           
            
            var result_even = 
                    Some(0)
                    .Bind(i => ParseInt(a)
                        .Bind(aa => 
                            ParseInt(b)
                            .Bind(bb => 
                                ParseInt(c)
                                    .Bind<int,int>(cc => Some(i + aa + bb + cc )))) )
                    ;
            
            // var x = from i in Some(0)
            //         from aa in ParseInt(a)
            //         from bb in ParseInt(b)
            //         from cc in ParseInt(c)
            //         select i + aa + bb + cc;

            Assert.Equal(Some(6), result_even);

        }

    }
}