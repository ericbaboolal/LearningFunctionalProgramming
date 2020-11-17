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
        public static Option<A> Filter<A>(this Option<A> input, Func<A, bool> f) =>
            // This will help you start out
            // 1) Remove this line
            //None<A>()
            // 2) Uncomment the following and Fill in the blanks
            
            
            //since the function we want to perform is bool we want to 
            //return the Option data type of 'input' based on whether the function
            //returned true or false (so we run a ternary).... 
            //data returned is either Some or None
            
            input.Match(
                Some: i => f(i) ? Some(i) : None<A>(),
                None: () => None<A>()
            );

        // Map :: Option<A> -> (A -> B) -> Option<B>
        public static Option<B> Map<A,B>(this Option<A> input, Func<A,B> f) =>
            
            //our goal is to take the Option input in and convert 
            //whatever's inside to a string. This string will be returned as an Option
            //Since we're converting here from one data type to another, we need
            //to make sure that the Option type returned<B> is different than the input
            //Option type<A>... I'm still forgetting a little bit about why the <A,B>
            //exists after the Map function name... we're specifying the data type the
            //function CAN take as a parameter? I'll have to re-ask Greg this.

            //What's interesting to not with the user supplied function signature 
            //is that your input parameter value is of datatype A and 
            //returning a datatype of B (unwrapped)


            //Also why are we scoping the first arg with 'this'? 

            //Some notes about what's happening below: we're running function f on
            //the the value that's inside Option<A> input. The function will return a
            //string that will need to wrapped as an Option<B> type for return.
            //In the case of None, we need to pass back a None wrapped in Option<B>

            input.Match(
                Some: i => Some<B>(f(i)),
                None: () => None<B>()
            );
        

        // Bind :: Option<A> -> (A -> Option<B>) -> Option<B>

        //So the big difference here as noted above in the hint is that the user
        //defined function that gets passed the input parameter value is of datatype A and
        //return Parameter is of datatype Option B. This difference is still a little fuzzy 
        //to me but has to do with fact that Map will add a wrapper automatically and
        //Bind will assume that the user will take care of that. -- I need to ask Greg 
        //if he can help with a little story time on this again so I can cement in the "why" a little better


        public static Option<B> Bind<A,B>(this Option<A> input, Func<A,Option<B>> f ) =>
            input.Match(
                Some: i => f(i),
                None: () => None<B>()
            );
        

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
                .Filter(i => i % 2 == 0);
            Assert.Equal(Some(2), result_evens);
        }
        [Fact]
        public void Filter_Odds()
        {
            var result_odds = Some(2)
                .Filter(i => i % 2 == 1);
            Assert.Equal(None<int>(), result_odds);
        }
        [Fact]
        public void Filter_None()
        {
            var always_true = None<string>()              
                .Filter(i => true);
            Assert.Equal(None<string>(), always_true);

            var always_false = None<string>()
                .Filter(i => false);
            Assert.Equal(None<string>(), always_false);
        }
    }
    public class OptionMapTests
    {
        [Fact]
        public void Map_Int_to_String()
        {
            var result = Some(1)          
                .Map(i => $"Value: {i.ToString()}");
            Assert.Equal(Some("Value: 1"), result);
        }
        [Fact]
        public void Map_None()
        {
            var result = None<string>()        
                .Map(i => $"Value: {i}");
            Assert.Equal(None<string>(), result);
        }
        [Fact]
        public void Map_Nested()
        {
            var result_even = Some(2)             
                .Map(i => i % 2 == 0 ? Some(i.ToString()) : None<string>());            
                Assert.Equal(Some(Some("2")), result_even);

            var result_odd = Some(1)
                .Map(i => i % 2 == 0 ? Some(i.ToString()) : None<string>());
            Assert.Equal(Some(None<string>()), result_odd);
        }
    }

    public class OptionBindTests
    {
        [Fact]
        public void Bind_Int_to_String()
        {
            var result = Some(1)
                .Bind(i => Some($"Value: {i}"))                ;
            Assert.Equal(Some("Value: 1"), result);
        }
        [Fact]
        public void Bind_None()
        {
            var result = None<string>()
            .Bind(i => Some($"Value: {i}"));
            Assert.Equal(None<string>(), result);
        }
        [Fact]
        public void Bind_Nested()
        {
            var result_even = Some(2)
                .Bind(i => i % 2 == 0 ? Some(i.ToString()) : None<string>());
            Assert.Equal(Some("2"), result_even);

            var result_odd = Some(1)
                .Bind(i => i % 2 == 0 ? Some(i.ToString()) : None<string>());
            Assert.Equal(None<string>(), result_odd);
        }
    }
}