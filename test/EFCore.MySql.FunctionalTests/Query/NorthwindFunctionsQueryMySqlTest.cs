using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.Query
{
    public partial class NorthwindFunctionsQueryMySqlTest : NorthwindFunctionsQueryRelationalTestBase<
        CaseSensitiveNorthwindQueryMySqlFixture<NoopModelCustomizer>>
    {
        protected static string LocalMethod1()
            => "M";

        protected static string LocalMethod2()
            => "m";

        public NorthwindFunctionsQueryMySqlTest(
            CaseSensitiveNorthwindQueryMySqlFixture<NoopModelCustomizer> fixture,
            ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            ClearLog();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_Literal(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.StartsWith("M")));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE 'M%'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_Identity(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.StartsWith(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND (LEFT(`c`.`ContactName`, CHAR_LENGTH(`c`.`ContactName`)) = `c`.`ContactName`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_Column(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.StartsWith(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND (LEFT(`c`.`ContactName`, CHAR_LENGTH(`c`.`ContactName`)) = `c`.`ContactName`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_MethodCall(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.StartsWith(LocalMethod1())));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE 'M%'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_Literal(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith("b")));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE '%b'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_Identity(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND (RIGHT(`c`.`ContactName`, CHAR_LENGTH(`c`.`ContactName`)) = `c`.`ContactName`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_Column(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND (RIGHT(`c`.`ContactName`, CHAR_LENGTH(`c`.`ContactName`)) = `c`.`ContactName`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_MethodCall(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith(LocalMethod2())));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE '%m'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_Literal(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M")));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE '%M%'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_Identity(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Contains(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND ((LOCATE(`c`.`ContactName`, `c`.`ContactName`) > 0) OR (`c`.`ContactName` LIKE ''))
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_Column(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CompanyName.Contains(c.ContactName)));

        AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NOT NULL AND ((LOCATE(`c`.`ContactName`, `c`.`CompanyName`) > 0) OR (`c`.`ContactName` LIKE ''))
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_MethodCall(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Contains(LocalMethod1())));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE '%M%'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrWhiteSpace_in_predicate(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => string.IsNullOrWhiteSpace(c.Region)));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`Region` IS NULL OR (TRIM(`c`.`Region`) = '')");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Indexof_with_emptystring(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.Region.IndexOf(string.Empty) == 0), ss => ss.Set<Customer>().Where(c => c.Region != null && c.Region.IndexOf(string.Empty) == 0) );

        AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (LOCATE('', `c`.`Region`) - 1) = 0
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Replace_with_emptystring(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Replace("ia", string.Empty) == "Mar Anders"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE REPLACE(`c`.`ContactName`, 'ia', '') = 'Mar Anders'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_one_arg_with_zero_startindex(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>() .Where(c => c.CustomerID.Substring(0) == "ALFKI") .Select(c => c.ContactName));

            AssertSql(
                @"SELECT `c`.`ContactName`
FROM `Customers` AS `c`
WHERE SUBSTRING(`c`.`CustomerID`, 0 + 1, CHAR_LENGTH(`c`.`CustomerID`)) = 'ALFKI'");
        }

        [ConditionalFact]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_one_arg_with_constant()
        {

            AssertSql(
                @"SELECT `c`.`ContactName`
FROM `Customers` AS `c`
WHERE SUBSTRING(`c`.`CustomerID`, 1 + 1, CHAR_LENGTH(`c`.`CustomerID`)) = 'LFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_one_arg_with_closure(bool async)
        {
                var start = 2;

                await AssertQuery(
                    async,
                    ss => ss.Set<Customer>()
                        .Where(c => c.CustomerID.Substring(start) == "FKI")
                        .Select(c => c.ContactName));

            AssertSql(
                @"@__start_0='2'

SELECT `c`.`ContactName`
FROM `Customers` AS `c`
WHERE SUBSTRING(`c`.`CustomerID`, @__start_0 + 1, CHAR_LENGTH(`c`.`CustomerID`)) = 'FKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_two_args_with_zero_startindex(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CustomerID == "ALFKI").Select(c => c.ContactName.Substring(0, 3)));

            AssertSql(
                @"SELECT SUBSTRING(`c`.`ContactName`, 0 + 1, 3)
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ALFKI'");
        }

        [ConditionalFact]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_two_args_with_zero_length()
        {

            AssertSql(
                @"SELECT SUBSTRING(`c`.`ContactName`, 2 + 1, 0)
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ALFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_two_args_with_constant(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CustomerID == "ALFKI").Select(c => c.ContactName.Substring(1, 3)));

            AssertSql(
                @"SELECT SUBSTRING(`c`.`ContactName`, 1 + 1, 3)
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ALFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_two_args_with_closure(bool async)
        {
                var start = 2;

                await AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.CustomerID == "ALFKI").Select(c => c.ContactName.Substring(start, 3)));

            AssertSql(
                @"@__start_0='2'

SELECT SUBSTRING(`c`.`ContactName`, @__start_0 + 1, 3)
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ALFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Substring_with_two_args_with_Index_of(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CustomerID == "ALFKI") .Select(c => c.ContactName.Substring(c.ContactName.IndexOf("a"), 3)));

            AssertSql(
                @"SELECT SUBSTRING(`c`.`ContactName`, (LOCATE('a', `c`.`ContactName`) - 1) + 1, 3)
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ALFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Regex_IsMatch_MethodCall(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(o => Regex.IsMatch(o.CustomerID, "^T")));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` REGEXP '^T'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Regex_IsMatch_MethodCall_constant_input(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(o => Regex.IsMatch("ALFKI", o.CustomerID)));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE 'ALFKI' REGEXP `c`.`CustomerID`");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_abs1(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Product>() .Where(od => Math.Abs(od.ProductID) > 10));

            AssertSql(
                @"SELECT `p`.`ProductID`, `p`.`Discontinued`, `p`.`ProductName`, `p`.`SupplierID`, `p`.`UnitPrice`, `p`.`UnitsInStock`
FROM `Products` AS `p`
WHERE ABS(`p`.`ProductID`) > 10");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_abs2(bool async)
        {
            await AssertQuery( async, ss => ss.Set<OrderDetail>() .Where(od => od.UnitPrice < 7) .Where(od => Math.Abs(od.Quantity) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`UnitPrice` < 7.0) AND (ABS(`o`.`Quantity`) > 10)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_abs_uncorrelated(bool async)
        {
            await AssertQuery( async, ss => ss.Set<OrderDetail>() .Where(od => od.UnitPrice < 7) .Where(od => Math.Abs(-10) < od.ProductID));
            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`UnitPrice` < 7.0) AND (10 < `o`.`ProductID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_math_round_int(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Order>() .Where(o => o.OrderID < 10250) .Select(o => new { A = Math.Round((double)o.OrderID) }), e => e.A);

            AssertSql(
                $@"SELECT ROUND({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}) AS `A`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_min(bool async)
        {
            await AssertQuery( async, ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077) .Where(od => Math.Min(od.OrderID, od.ProductID) == od.ProductID));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (LEAST(`o`.`OrderID`, `o`.`ProductID`) = `o`.`ProductID`)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_max(bool async)
        {
            await AssertQuery( async, ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Max(od.OrderID, od.ProductID) == od.OrderID));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (GREATEST(`o`.`OrderID`, `o`.`ProductID`) = `o`.`OrderID`)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_string_to_lower(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CustomerID.ToLower() == "alfki"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE LOWER(`c`.`CustomerID`) = 'alfki'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_string_to_upper(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CustomerID.ToUpper() == "ALFKI"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE UPPER(`c`.`CustomerID`) = 'ALFKI'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task TrimStart_without_arguments_in_predicate(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimStart() == "Owner"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(LEADING FROM `c`.`ContactTitle`) = 'Owner'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task TrimStart_with_char_argument_in_predicate(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimStart('O') == "wner"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(LEADING 'O' FROM `c`.`ContactTitle`) = 'wner'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task TrimStart_with_char_array_argument_in_predicate(bool async)
        {
            // MySQL only supports a string (characters in fixed order) as the parameter specifying what should be trimmed.
            // String.TrimStart has a different behavior, where any single character in any order will be trimmed.
            // Therefore, calling String.TrimStart with more than one char to trim, triggers client eval.
            return Assert.ThrowsAsync<InvalidOperationException>(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimStart(new [] {'O', 'w'}) == "ner"))));
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task TrimEnd_without_arguments_in_predicate(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimEnd() == "Owner"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(TRAILING FROM `c`.`ContactTitle`) = 'Owner'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task TrimEnd_with_char_argument_in_predicate(bool async)
        {
            await AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimEnd('r') == "Owne"));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(TRAILING 'r' FROM `c`.`ContactTitle`) = 'Owne'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task TrimEnd_with_char_array_argument_in_predicate(bool async)
        {
            // MySQL only supports a string (characters in fixed order) as the parameter specifying what should be trimmed.
            // String.TrimEnd has a different behavior, where any single character in any order will be trimmed.
            // Therefore, calling String.TrimEnd with more than one char to trim, triggers client eval.
            return Assert.ThrowsAsync<InvalidOperationException>(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.TrimEnd(new [] {'e', 'r'}) == "Own"))));
        }

        [ConditionalFact]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Trim_without_argument_in_predicate()
        {

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(`c`.`ContactTitle`) = 'Owner'");
        }

        [ConditionalFact]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Trim_with_char_argument_in_predicate()
        {

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM('O' FROM `c`.`ContactTitle`) = 'wner'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Trim_with_char_array_argument_in_predicate(bool async)
        {
            // MySQL only supports a string (characters in fixed order) as the parameter specifying what should be trimmed.
            // String.Trim has a different behavior, where any single character in any order will be trimmed.
            // Therefore, calling String.Trim with more than one char to trim, triggers client eval.
            return Assert.ThrowsAsync<InvalidOperationException>(() => AssertQuery(async, ss => ss.Set<Customer>().Where(c => c.ContactTitle.Trim(new[] { 'O', 'r' }) == "wne")));
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_FirstOrDefault_MethodCall(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.FirstOrDefault() == 'A'));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE SUBSTRING(`c`.`ContactName`, 1, 1) = 'A'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_constant_with_whitespace(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("     ")),
            assertEmpty: true);

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE '%     %'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_parameter_with_whitespace(bool async)
        {

        var pattern = "     ";
        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.Contains(pattern)),
            assertEmpty: true);

        AssertSql(
"""
@__pattern_0_contains='%     %' (Size = 30)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE @__pattern_0_contains
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_LastOrDefault_MethodCall(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.LastOrDefault() == 's'));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE SUBSTRING(`c`.`ContactName`, CHAR_LENGTH(`c`.`ContactName`), 1) = 's'");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_abs3(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => Math.Abs(od.UnitPrice) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (ABS(`o`.`UnitPrice`) > 10.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_ceiling1(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.UnitPrice < 7)
                .Where(od => Math.Ceiling(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`UnitPrice` < 7.0) AND (CEILING({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_ceiling2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => Math.Ceiling(od.UnitPrice) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (CEILING(`o`.`UnitPrice`) > 10.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_floor(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => Math.Floor(od.UnitPrice) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (FLOOR(`o`.`UnitPrice`) > 10.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_power(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => Math.Pow(od.Discount, 3) > 0.005f));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE POWER({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}, 3.0) > 0.004999999888241291");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_round(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => Math.Round(od.UnitPrice) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (ROUND(`o`.`UnitPrice`) > 10.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_math_truncate_int(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>()
                .Where(o => o.OrderID < 10250)
                .Select(o => new { A = Math.Truncate((double)o.OrderID) }),
            e => e.A);

            AssertSql(
                $@"SELECT TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) AS `A`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_round2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => Math.Round(od.UnitPrice, 2) > 100));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ROUND(`o`.`UnitPrice`, 2) > 100.0");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_truncate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => Math.Truncate(od.UnitPrice) > 10));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (TRUNCATE(`o`.`UnitPrice`, 0) > 10.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_exp(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Exp(od.Discount) > 1));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (EXP({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 1.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_log10(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => Math.Log10(od.Discount) < 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG10({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) < 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_log(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => Math.Log(od.Discount) < 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) < 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_log_new_base(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => Math.Log(od.Discount, 7) < -1));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG(7.0, {MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) < -1.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_sqrt(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Sqrt(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SQRT({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_acos(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Acos(od.Discount) > 1));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ACOS({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 1.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_asin(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Asin(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ASIN({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_atan(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Atan(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ATAN({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_atan2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Atan2(od.Discount, 1) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ATAN2({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}, 1.0) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_cos(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Cos(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (COS({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_sin(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Sin(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SIN({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_tan(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Tan(od.Discount) > 0));

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (TAN({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_sign(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => Math.Sign(od.Discount) > 0));

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SIGN(`o`.`Discount`) > 0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_guid_newguid(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>()
                .Where(od => Guid.NewGuid() != default));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE UUID() <> '00000000-0000-0000-0000-000000000000'");
        }

        public override async Task Where_functions_nested(bool async)
        {
            await base.Where_functions_nested(async);

            AssertSql(
                $@"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE POWER({MySqlTestHelpers.CastAsDouble("CHAR_LENGTH(`c`.`CustomerID`)")}, 2.0) = 25.0");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrEmpty_in_predicate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.IsNullOrEmpty(c.Region)));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`Region` IS NULL OR (`c`.`Region` = '')");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrEmpty_in_projection(bool async)
        {
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrEmpty_negated_in_projection(bool async)
        {
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrWhiteSpace_in_predicate_on_non_nullable_column(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.IsNullOrWhiteSpace(c.CustomerID)),
            assertEmpty: true);

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE TRIM(`c`.`CustomerID`) = ''");
        }

        public override async Task Order_by_length_twice(bool async)
        {
            await base.Order_by_length_twice(async);

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
ORDER BY CHAR_LENGTH(`c`.`CustomerID`), `c`.`CustomerID`");
        }

        public override async Task Order_by_length_twice_followed_by_projection_of_naked_collection_navigation(bool async)
        {
            await base.Order_by_length_twice_followed_by_projection_of_naked_collection_navigation(async);

            AssertSql(
                @"SELECT `c`.`CustomerID`, `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Customers` AS `c`
LEFT JOIN `Orders` AS `o` ON `c`.`CustomerID` = `o`.`CustomerID`
ORDER BY CHAR_LENGTH(`c`.`CustomerID`), `c`.`CustomerID`");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Static_string_equals_in_predicate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Equals(c.CustomerID, "ANATR")));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'ANATR'");
        }

        public override async Task Static_equals_nullable_datetime_compared_to_non_nullable(bool async)
        {
            await base.Static_equals_nullable_datetime_compared_to_non_nullable(async);

            AssertSql(
                @"@__arg_0='1996-07-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` = @__arg_0");
        }

        public override async Task Static_equals_int_compared_to_long(bool async)
        {
            await base.Static_equals_int_compared_to_long(async);

            AssertSql(
                @"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE FALSE");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Projecting_Math_Truncate_and_ordering_by_it_twice(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>().Where(o => o.OrderID < 10250).Select(o => new { A = Math.Truncate((double)o.OrderID) })
                .OrderBy(r => r.A)
                .OrderBy(r => r.A),
            assertOrder: true);

            AssertSql(
                $@"SELECT TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) AS `A`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250
ORDER BY TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Projecting_Math_Truncate_and_ordering_by_it_twice2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>().Where(o => o.OrderID < 10250).Select(o => new { A = Math.Truncate((double)o.OrderID) })
                .OrderBy(r => r.A)
                .OrderByDescending(r => r.A),
            assertOrder: true);

            AssertSql(
                $@"SELECT TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) AS `A`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250
ORDER BY TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) DESC");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Projecting_Math_Truncate_and_ordering_by_it_twice3(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>().Where(o => o.OrderID < 10250).Select(o => new { A = Math.Truncate((double)o.OrderID) })
                .OrderByDescending(r => r.A)
                .ThenBy(r => r.A),
            assertOrder: true);

            AssertSql(
                $@"SELECT TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) AS `A`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250
ORDER BY TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")}, 0) DESC");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_simple_zero(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") == 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 != string.Compare(c.CustomerID, "AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") > 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 >= string.Compare(c.CustomerID, "AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 < string.Compare(c.CustomerID, "AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") <= 0));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <> 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_simple_one(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") == 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 == string.Compare(c.CustomerID, "AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") < 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 > string.Compare(c.CustomerID, "AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "AROUT") > -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 < string.Compare(c.CustomerID, "AROUT")));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= 'AROUT'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_compare_with_parameter(bool async)
        {

        Customer customer = null;
        using (var context = CreateContext())
        {
            customer = await context.Customers.SingleAsync(c => c.CustomerID == "AROUT");
        }

        ClearLog();

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, customer.CustomerID) == 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 == string.Compare(c.CustomerID, customer.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, customer.CustomerID) < 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 > string.Compare(c.CustomerID, customer.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, customer.CustomerID) > -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 < string.Compare(c.CustomerID, customer.CustomerID)));

            AssertSql(
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= @__customer_CustomerID_0
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_simple_more_than_one(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "ALFKI") == 42),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "ALFKI") > 42),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 42 > string.Compare(c.CustomerID, "ALFKI")));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END = 42",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END > 42",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE 42 > CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_nested(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "M" + c.CustomerID) == 0),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 != string.Compare(c.CustomerID, c.CustomerID.ToUpper())),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "ALFKI".Replace("ALF".ToUpper(), c.CustomerID)) > 0),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 >= string.Compare(c.CustomerID, "M" + c.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 == string.Compare(c.CustomerID, c.CustomerID.ToUpper())),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "ALFKI".Replace("ALF".ToUpper(), c.CustomerID)) == -1));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = (CONCAT('M', `c`.`CustomerID`))",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <> UPPER(`c`.`CustomerID`)",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > REPLACE('ALFKI', 'ALF', `c`.`CustomerID`)",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= (CONCAT('M', `c`.`CustomerID`))",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > UPPER(`c`.`CustomerID`)",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < REPLACE('ALFKI', 'ALF', `c`.`CustomerID`)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_multi_predicate(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.CustomerID, "ALFKI") > -1)
                .Where(c => string.Compare(c.CustomerID, "CACTU") == -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Compare(c.ContactTitle, "Owner") == 0)
                .Where(c => string.Compare(c.Country, "USA") != 0));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (`c`.`CustomerID` >= 'ALFKI') AND (`c`.`CustomerID` < 'CACTU')",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (`c`.`ContactTitle` = 'Owner') AND ((`c`.`Country` <> 'USA') OR `c`.`Country` IS NULL)");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_to_simple_zero(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") == 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 != c.CustomerID.CompareTo("AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") > 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 >= c.CustomerID.CompareTo("AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 < c.CustomerID.CompareTo("AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") <= 0));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <> 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_to_simple_one(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") == 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 == c.CustomerID.CompareTo("AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") < 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 > c.CustomerID.CompareTo("AROUT")));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT") > -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 < c.CustomerID.CompareTo("AROUT")));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= 'AROUT'
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= 'AROUT'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_compare_to_with_parameter(bool async)
        {

        Customer customer = null;
        using (var context = CreateContext())
        {
            customer = await context.Customers.SingleAsync(x => x.CustomerID == "AROUT");
        }

        ClearLog();

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo(customer.CustomerID) == 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 == c.CustomerID.CompareTo(customer.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo(customer.CustomerID) < 1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 > c.CustomerID.CompareTo(customer.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo(customer.CustomerID) > -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => -1 < c.CustomerID.CompareTo(customer.CustomerID)));

            AssertSql(
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= @__customer_CustomerID_0
""",
                //
"""
@__customer_CustomerID_0='AROUT' (Size = 5) (DbType = StringFixedLength)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` >= @__customer_CustomerID_0
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_to_simple_more_than_one(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("ALFKI") == 42),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("ALFKI") > 42),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 42 > c.CustomerID.CompareTo("ALFKI")));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END = 42",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END > 42",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE 42 > CASE
    WHEN `c`.`CustomerID` = 'ALFKI' THEN 0
    WHEN `c`.`CustomerID` > 'ALFKI' THEN 1
    WHEN `c`.`CustomerID` < 'ALFKI' THEN -1
END");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_to_nested(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("M" + c.CustomerID) != 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 == c.CustomerID.CompareTo(c.CustomerID.ToUpper())));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT".Replace("OUT".ToUpper(), c.CustomerID)) > 0));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 0 >= c.CustomerID.CompareTo("M" + c.CustomerID)));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => 1 == c.CustomerID.CompareTo(c.CustomerID.ToUpper())),
            assertEmpty: true);

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("AROUT".Replace("OUT".ToUpper(), c.CustomerID)) == -1));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <> (CONCAT('M', `c`.`CustomerID`))
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` = UPPER(`c`.`CustomerID`)
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > REPLACE('AROUT', 'OUT', `c`.`CustomerID`)
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` <= (CONCAT('M', `c`.`CustomerID`))
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` > UPPER(`c`.`CustomerID`)
""",
                //
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`CustomerID` < REPLACE('AROUT', 'OUT', `c`.`CustomerID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Compare_to_multi_predicate(bool async)
        {

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.CustomerID.CompareTo("ALFKI") > -1).Where(c => c.CustomerID.CompareTo("CACTU") == -1));

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactTitle.CompareTo("Owner") == 0).Where(c => c.Country.CompareTo("USA") != 0));

            AssertSql(
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (`c`.`CustomerID` >= 'ALFKI') AND (`c`.`CustomerID` < 'CACTU')",
                //
                @"SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (`c`.`ContactTitle` = 'Owner') AND ((`c`.`Country` <> 'USA') OR `c`.`Country` IS NULL)");
        }

        [ConditionalTheory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public virtual async Task DateTime_Compare_to_simple_zero(bool async, bool compareTo)
        {
            var myDatetime = new DateTime(1998, 5, 4);

            if (compareTo)
            {
                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) == 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 != c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) > 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 >= c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 < c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) <= 0));
            }
            else
            {
                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) == 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 != DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) > 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 >= DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 < DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) <= 0));
            }

            AssertSql(
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` = @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`OrderDate` <> @__myDatetime_0) OR `o`.`OrderDate` IS NULL",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` > @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` <= @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` > @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` <= @__myDatetime_0");
        }

        [ConditionalTheory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public virtual async Task TimeSpan_Compare_to_simple_zero(bool async, bool compareTo)
        {
            var myDatetime = new DateTime(1998, 5, 4);

            if (compareTo)
            {
                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) == 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 != c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) > 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 >= c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 < c.OrderDate.Value.CompareTo(myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => c.OrderDate.Value.CompareTo(myDatetime) <= 0));
            }
            else
            {
                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) == 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 != DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) > 0));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 >= DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => 0 < DateTime.Compare(c.OrderDate.Value, myDatetime)));

                await AssertQuery(
                    async,
                    ss => ss.Set<Order>().Where(c => DateTime.Compare(c.OrderDate.Value, myDatetime) <= 0));
            }

            AssertSql(
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` = @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`OrderDate` <> @__myDatetime_0) OR `o`.`OrderDate` IS NULL",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` > @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` <= @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` > @__myDatetime_0",
                //
                @"@__myDatetime_0='1998-05-04T00:00:00.0000000' (DbType = DateTime)

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` <= @__myDatetime_0");
        }

        public virtual async Task Int_Compare_to_simple_zero()
        {

            AssertSql(
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` = @__orderId_0",
                //
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` <> @__orderId_0",
                //
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` > @__orderId_0",
                //
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` <= @__orderId_0",
                //
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` > @__orderId_0",
                //
                @"@__orderId_0='10250'

SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` <= @__orderId_0");
        }

        public virtual async Task Convert_ToBoolean()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(`o`.`OrderID` % 3 AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(CAST(`o`.`OrderID` % 3 AS unsigned) AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(CAST(`o`.`OrderID` % 3 AS decimal(65,30)) AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(CAST(`o`.`OrderID` % 3 AS double) AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(CAST(`o`.`OrderID` % 3 AS double) AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(`o`.`OrderID` % 3 AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(`o`.`OrderID` % 3 AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(`o`.`OrderID` % 3 AS signed)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND CAST(`o`.`OrderID` % 3 AS signed)
""");
        }

        public virtual async Task Convert_ToByte()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS decimal(65,30)) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS unsigned) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS unsigned) >= 0)
""");
        }

        public virtual async Task Convert_ToDecimal()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS unsigned) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS decimal(65,30)) >= 0.0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS decimal(65,30)) >= 0.0)
""");
        }

        public virtual async Task Convert_ToDouble()
        {

            AssertSql(
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS signed)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS unsigned)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS decimal(65,30))")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("`o`.`OrderID` % 1")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("`o`.`OrderID` % 1")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS signed)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS signed)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS signed)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS char)")} >= 0.0)",
                //
                $@"SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ({MySqlTestHelpers.CastAsDouble("CAST(`o`.`OrderID` % 1 AS char)")} >= 0.0)");
        }

        public virtual async Task Convert_ToInt16()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS unsigned) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS decimal(65,30)) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""");
        }

        public virtual async Task Convert_ToInt32()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS unsigned) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS decimal(65,30)) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""");
        }

        public virtual async Task Convert_ToInt64()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS unsigned) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS decimal(65,30)) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS char) AS signed) >= 0)
""");
        }

        public virtual async Task Convert_ToString()
        {

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS unsigned) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS decimal(65,30)) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS double) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(CAST(`o`.`OrderID` % 1 AS signed) AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND (CAST(`o`.`OrderID` % 1 AS char) <> '10')
""",
                //
                """
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (`o`.`CustomerID` = 'ALFKI') AND ((CAST(`o`.`OrderDate` AS char) LIKE '%1997%') OR (CAST(`o`.`OrderDate` AS char) LIKE '%1998%'))
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_Parameter(bool async)
        {

        var pattern = "M";

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.StartsWith(pattern)));

        AssertSql(
"""
@__pattern_0_startswith='M%' (Size = 30)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE @__pattern_0_startswith
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_Parameter(bool async)
        {

        var pattern = "b";

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith(pattern)));

        AssertSql(
"""
@__pattern_0_endswith='%b' (Size = 30)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` LIKE @__pattern_0_endswith
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Join_over_non_nullable_column(bool async)
        {
await  AssertQuery(
            async,
            ss => ss.Set<Customer>()
                .GroupBy(c => c.City)
                .Select(g => new { City = g.Key, Customers = string.Join("|", g.Select(e => e.CustomerID)) }),
            elementSorter: x => x.City,
            elementAsserter: (e, a) =>
            {
                Assert.Equal(e.City, a.City);

                // Ordering inside the string isn't specified server-side, split and reorder
                Assert.Equal(
                    e.Customers.Split("|").OrderBy(id => id).ToArray(),
                    a.Customers.Split("|").OrderBy(id => id).ToArray());
            });

        AssertSql(
"""
SELECT `c1`.`City`, `c0`.`CustomerID`
FROM (
    SELECT `c`.`City`
    FROM `Customers` AS `c`
    GROUP BY `c`.`City`
) AS `c1`
LEFT JOIN `Customers` AS `c0` ON `c1`.`City` = `c0`.`City`
ORDER BY `c1`.`City`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Join_with_predicate(bool async)
        {
await  AssertQuery(
            async,
            ss => ss.Set<Customer>()
                .GroupBy(c => c.City)
                .Select(
                    g => new
                    {
                        City = g.Key, Customers = string.Join("|", g.Where(e => e.ContactName.Length > 10).Select(e => e.CustomerID))
                    }),
            elementSorter: x => x.City,
            elementAsserter: (e, a) =>
            {
                Assert.Equal(e.City, a.City);

                // Ordering inside the string isn't specified server-side, split and reorder
                Assert.Equal(
                    e.Customers.Split("|").OrderBy(id => id).ToArray(),
                    a.Customers.Split("|").OrderBy(id => id).ToArray());
            });

        AssertSql(
"""
SELECT `c1`.`City`, `c2`.`CustomerID`
FROM (
    SELECT `c`.`City`
    FROM `Customers` AS `c`
    GROUP BY `c`.`City`
) AS `c1`
LEFT JOIN (
    SELECT `c0`.`CustomerID`, `c0`.`City`
    FROM `Customers` AS `c0`
    WHERE CHAR_LENGTH(`c0`.`ContactName`) > 10
) AS `c2` ON `c1`.`City` = `c2`.`City`
ORDER BY `c1`.`City`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Join_with_ordering(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>()
                .GroupBy(c => c.City)
                .Select(
                    g => new
                    {
                        City = g.Key, Customers = string.Join("|", g.OrderByDescending(e => e.CustomerID).Select(e => e.CustomerID))
                    }),
            elementSorter: x => x.City);

        AssertSql(
"""
SELECT `c1`.`City`, `c0`.`CustomerID`
FROM (
    SELECT `c`.`City`
    FROM `Customers` AS `c`
    GROUP BY `c`.`City`
) AS `c1`
LEFT JOIN `Customers` AS `c0` ON `c1`.`City` = `c0`.`City`
ORDER BY `c1`.`City`, `c0`.`CustomerID` DESC
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Join_over_nullable_column(bool async)
        {
await  AssertQuery(
            async,
            ss => ss.Set<Customer>()
                .GroupBy(c => c.City)
                .Select(g => new { City = g.Key, Regions = string.Join("|", g.Select(e => e.Region)) }),
            elementSorter: x => x.City,
            elementAsserter: (e, a) =>
            {
                Assert.Equal(e.City, a.City);

                // Ordering inside the string isn't specified server-side, split and reorder
                Assert.Equal(
                    e.Regions.Split("|").OrderBy(id => id).ToArray(),
                    a.Regions.Split("|").OrderBy(id => id).ToArray());
            });

        AssertSql(
"""
SELECT `c1`.`City`, `c0`.`Region`, `c0`.`CustomerID`
FROM (
    SELECT `c`.`City`
    FROM `Customers` AS `c`
    GROUP BY `c`.`City`
) AS `c1`
LEFT JOIN `Customers` AS `c0` ON `c1`.`City` = `c0`.`City`
ORDER BY `c1`.`City`
""");
        }

        public virtual async Task String_Concat()
        {

        AssertSql(
"""
SELECT `c1`.`City`, `c0`.`CustomerID`
FROM (
    SELECT `c`.`City`
    FROM `Customers` AS `c`
    GROUP BY `c`.`City`
) AS `c1`
LEFT JOIN `Customers` AS `c0` ON `c1`.`City` = `c0`.`City`
ORDER BY `c1`.`City`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_square(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => Math.Pow(od.Discount, 2) > 0.05f));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE POWER({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}, 2.0) > 0.05000000074505806
""");
        }

        public override async Task Sum_over_round_works_correctly_in_projection(bool async)
        {
            await base.Sum_over_round_works_correctly_in_projection(async);

            AssertSql(
"""
SELECT `o`.`OrderID`, (
    SELECT COALESCE(SUM(ROUND(`o0`.`UnitPrice`, 2)), 0.0)
    FROM `Order Details` AS `o0`
    WHERE `o`.`OrderID` = `o0`.`OrderID`) AS `Sum`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10300
""");
        }

        public override async Task Sum_over_round_works_correctly_in_projection_2(bool async)
        {
            await base.Sum_over_round_works_correctly_in_projection_2(async);

            AssertSql(
"""
SELECT `o`.`OrderID`, (
    SELECT COALESCE(SUM(ROUND(`o0`.`UnitPrice` * `o0`.`UnitPrice`, 2)), 0.0)
    FROM `Order Details` AS `o0`
    WHERE `o`.`OrderID` = `o0`.`OrderID`) AS `Sum`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10300
""");
        }

        public override async Task Sum_over_truncate_works_correctly_in_projection(bool async)
        {
            await base.Sum_over_truncate_works_correctly_in_projection(async);

            AssertSql(
"""
SELECT `o`.`OrderID`, (
    SELECT COALESCE(SUM(TRUNCATE(`o0`.`UnitPrice`, 0)), 0.0)
    FROM `Order Details` AS `o0`
    WHERE `o`.`OrderID` = `o0`.`OrderID`) AS `Sum`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10300
""");
        }

        public override async Task Sum_over_truncate_works_correctly_in_projection_2(bool async)
        {
            await base.Sum_over_truncate_works_correctly_in_projection_2(async);

            AssertSql(
"""
SELECT `o`.`OrderID`, (
    SELECT COALESCE(SUM(TRUNCATE(`o0`.`UnitPrice` * `o0`.`UnitPrice`, 0)), 0.0)
    FROM `Order Details` AS `o0`
    WHERE `o`.`OrderID` = `o0`.`OrderID`) AS `Sum`
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10300
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_degrees(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => double.RadiansToDegrees(od.Discount) > 0));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (DEGREES({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_radians(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => double.DegreesToRadians(od.Discount) > 0));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (RADIANS({MySqlTestHelpers.CastAsDouble("`o`.`Discount`")}) > 0.0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_abs1(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Product>()
                .Where(od => MathF.Abs(od.ProductID) > 10));

            AssertSql(
$"""
SELECT `p`.`ProductID`, `p`.`Discontinued`, `p`.`ProductName`, `p`.`SupplierID`, `p`.`UnitPrice`, `p`.`UnitsInStock`
FROM `Products` AS `p`
WHERE ABS({MySqlTestHelpers.CastAsDouble("`p`.`ProductID`")}) > 10
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_ceiling1(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.UnitPrice < 7)
                .Where(od => MathF.Ceiling(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`UnitPrice` < 7.0) AND (CEILING(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_floor(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => MathF.Floor((float)od.UnitPrice) > 10));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (FLOOR({MySqlTestHelpers.CastAsDouble("`o`.`UnitPrice`")}) > 10)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_power(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => MathF.Pow(od.Discount, 3) > 0.005f));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE POWER(`o`.`Discount`, 3) > 0.005
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_square(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => MathF.Pow(od.Discount, 2) > 0.05f));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE POWER(`o`.`Discount`, 2) > 0.05
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_round2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => MathF.Round((float)od.UnitPrice, 2) > 100));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ROUND({MySqlTestHelpers.CastAsDouble("`o`.`UnitPrice`")}, 2) > 100
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_mathf_round(bool async)
        {

await  AssertQueryScalar(
            async,
            ss => ss.Set<Order>()
                .Where(o => o.OrderID < 10250)
                .Select(o => MathF.Round(o.OrderID)));

            AssertSql(
$"""
SELECT ROUND({MySqlTestHelpers.CastAsDouble("`o`.`OrderID`")})
FROM `Orders` AS `o`
WHERE `o`.`OrderID` < 10250
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_mathf_round2(bool async)
        {

await  AssertQueryScalar(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Select(od => MathF.Round((float)od.UnitPrice, 2)));

            AssertSql(
$"""
SELECT ROUND({MySqlTestHelpers.CastAsDouble("`o`.`UnitPrice`")}, 2)
FROM `Order Details` AS `o`
WHERE `o`.`Quantity` < 5
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_truncate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Where(od => MathF.Truncate((float)od.UnitPrice) > 10));

            AssertSql(
$"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`Quantity` < 5) AND (TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`UnitPrice`")}, 0) > 10)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_mathf_truncate(bool async)
        {

await  AssertQueryScalar(
            async,
            ss => ss.Set<OrderDetail>()
                .Where(od => od.Quantity < 5)
                .Select(od => MathF.Truncate((float)od.UnitPrice)));

            AssertSql(
$"""
SELECT TRUNCATE({MySqlTestHelpers.CastAsDouble("`o`.`UnitPrice`")}, 0)
FROM `Order Details` AS `o`
WHERE `o`.`Quantity` < 5
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_exp(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Exp(od.Discount) > 1));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (EXP(`o`.`Discount`) > 1)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_log10(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => MathF.Log10(od.Discount) < 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG10(`o`.`Discount`) < 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_log(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => MathF.Log(od.Discount) < 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG(`o`.`Discount`) < 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_log_new_base(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077 && od.Discount > 0).Where(od => MathF.Log(od.Discount, 7) < -1));

        AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE ((`o`.`OrderID` = 11077) AND (`o`.`Discount` > 0)) AND (LOG(7, `o`.`Discount`) < -1)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_sqrt(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Sqrt(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SQRT(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_acos(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Acos(od.Discount) > 1));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ACOS(`o`.`Discount`) > 1)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_asin(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Asin(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ASIN(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_atan(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Atan(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ATAN(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_atan2(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Atan2(od.Discount, 1) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (ATAN2(`o`.`Discount`, 1) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_cos(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Cos(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (COS(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_sin(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Sin(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SIN(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_tan(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Tan(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (TAN(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_sign(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => MathF.Sign(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (SIGN(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_degrees(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => float.RadiansToDegrees(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (DEGREES(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_mathf_radians(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077).Where(od => float.DegreesToRadians(od.Discount) > 0));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (RADIANS(`o`.`Discount`) > 0)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Indexof_with_one_constant_arg(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.IndexOf("a") == 1));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (LOCATE('a', `c`.`ContactName`) - 1) = 1
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Indexof_with_one_parameter_arg(bool async)
        {

        var pattern = "a";

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.IndexOf(pattern) == 1));

            AssertSql(
"""
@__pattern_0='a' (Size = 4000)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (LOCATE(@__pattern_0, `c`.`ContactName`) - 1) = 1
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Indexof_with_constant_starting_position(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.IndexOf("a", 2) == 4));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (LOCATE('a', `c`.`ContactName`, 3) - 1) = 4
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Indexof_with_parameter_starting_position(bool async)
        {

        var start = 2;

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => c.ContactName.IndexOf("a", start) == 4));

            AssertSql(
"""
@__start_0='2'

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE (LOCATE('a', `c`.`ContactName`, @__start_0 + 1) - 1) = 4
""");
        }

        public virtual async Task Replace_using_property_arguments()
        {

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE REPLACE(`c`.`ContactName`, `c`.`ContactName`, `c`.`CustomerID`) = `c`.`CustomerID`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task IsNullOrEmpty_negated_in_predicate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => !string.IsNullOrEmpty(c.Region)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`Region` IS NOT NULL AND (`c`.`Region` <> '')
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_DateOnly_FromDateTime(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>()
                .Where(o => o.OrderDate.HasValue && DateOnly.FromDateTime(o.OrderDate.Value) == new DateOnly(1996, 9, 16))
                .AsTracking(QueryTrackingBehavior.TrackAll));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE `o`.`OrderDate` IS NOT NULL AND (DATE(`o`.`OrderDate`) = DATE '1996-09-16')
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_with_StringComparison_Ordinal(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CompanyName.StartsWith("Qu", StringComparison.Ordinal)))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_with_StringComparison_OrdinalIgnoreCase(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.CompanyName.StartsWith("Qu", StringComparison.OrdinalIgnoreCase)))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_with_StringComparison_Ordinal(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith("DY", StringComparison.Ordinal)), assertEmpty: true)));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_with_StringComparison_OrdinalIgnoreCase(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith("DY", StringComparison.OrdinalIgnoreCase)))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_with_StringComparison_Ordinal(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.Ordinal)))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_with_StringComparison_OrdinalIgnoreCase(bool async)
        {

await  AssertTranslationFailed(() => (AssertQuery( async, ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.OrdinalIgnoreCase)))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_StartsWith_with_StringComparison_unsupported(bool async)
        {

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.CompanyName.StartsWith("Qu", StringComparison.CurrentCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.CurrentCultureIgnoreCase))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("Qu", StringComparison.InvariantCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("Qu", StringComparison.InvariantCultureIgnoreCase))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_EndsWith_with_StringComparison_unsupported(bool async)
        {

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.EndsWith("Qu", StringComparison.CurrentCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.CurrentCultureIgnoreCase))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("Qu", StringComparison.InvariantCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("Qu", StringComparison.InvariantCultureIgnoreCase))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_in_projection(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Select(c => new { Id = c.CustomerID, Value = c.CompanyName.Contains(c.ContactName) }),
            elementSorter: e => e.Id);

            AssertSql(
"""
SELECT `c`.`CustomerID` AS `Id`, `c`.`ContactName` IS NOT NULL AND ((LOCATE(`c`.`ContactName`, `c`.`CompanyName`) > 0) OR (`c`.`ContactName` LIKE '')) AS `Value`
FROM `Customers` AS `c`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_negated_in_predicate(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => !c.CompanyName.Contains(c.ContactName)));

            AssertSql(
"""
SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE `c`.`ContactName` IS NULL OR ((LOCATE(`c`.`ContactName`, `c`.`CompanyName`) <= 0) AND `c`.`ContactName` NOT LIKE '')
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_negated_in_projection(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Customer>().Select(c => new { Id = c.CustomerID, Value = !c.CompanyName.Contains(c.ContactName) }),
            elementSorter: e => e.Id);

            AssertSql(
"""
SELECT `c`.`CustomerID` AS `Id`, `c`.`ContactName` IS NULL OR ((LOCATE(`c`.`ContactName`, `c`.`CompanyName`) <= 0) AND `c`.`ContactName` NOT LIKE '') AS `Value`
FROM `Customers` AS `c`
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Contains_with_StringComparison_unsupported(bool async)
        {

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.CurrentCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.CurrentCultureIgnoreCase))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.InvariantCulture))));

        await AssertTranslationFailed(
            () =>
                AssertQuery(
                    async,
                    ss => ss.Set<Customer>().Where(c => c.ContactName.Contains("M", StringComparison.InvariantCultureIgnoreCase))));

            AssertSql();
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task String_Join_non_aggregate(bool async)
        {

        var foo = "foo";

        await AssertQuery(
            async,
            ss => ss.Set<Customer>().Where(c => string.Join("|", new[] { c.CompanyName, foo, null, "bar" }) == "Around the Horn|foo||bar"));

            AssertSql(
"""
@__foo_0='foo' (Size = 4000)

SELECT `c`.`CustomerID`, `c`.`Address`, `c`.`City`, `c`.`CompanyName`, `c`.`ContactName`, `c`.`ContactTitle`, `c`.`Country`, `c`.`Fax`, `c`.`Phone`, `c`.`PostalCode`, `c`.`Region`
FROM `Customers` AS `c`
WHERE CONCAT_WS('|', `c`.`CompanyName`, @__foo_0, '', 'bar') = 'Around the Horn|foo||bar'
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_max_nested(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077)
                .Where(od => Math.Max(od.OrderID, Math.Max(od.ProductID, 1)) == od.OrderID));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (GREATEST(`o`.`OrderID`, `o`.`ProductID`, 1) = `o`.`OrderID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_max_nested_twice(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077)
                .Where(od => Math.Max(Math.Max(1, Math.Max(od.OrderID, 2)), od.ProductID) == od.OrderID));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (GREATEST(1, `o`.`OrderID`, 2, `o`.`ProductID`) = `o`.`OrderID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_min_nested(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077)
                .Where(od => Math.Min(od.OrderID, Math.Min(od.ProductID, 99999)) == od.ProductID));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (LEAST(`o`.`OrderID`, `o`.`ProductID`, 99999) = `o`.`ProductID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Where_math_min_nested_twice(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<OrderDetail>().Where(od => od.OrderID == 11077)
                .Where(od => Math.Min(Math.Min(99999, Math.Min(od.OrderID, 99998)), od.ProductID) == od.ProductID));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`ProductID`, `o`.`Discount`, `o`.`Quantity`, `o`.`UnitPrice`
FROM `Order Details` AS `o`
WHERE (`o`.`OrderID` = 11077) AND (LEAST(99999, `o`.`OrderID`, 99998, `o`.`ProductID`) = `o`.`ProductID`)
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_ToString_IndexOf(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>().Where(x => x.OrderID.ToString().IndexOf("123") == -1));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (LOCATE('123', CAST(`o`.`OrderID` AS char)) - 1) = -1
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual async Task Select_IndexOf_ToString(bool async)
        {

await  AssertQuery(
            async,
            ss => ss.Set<Order>().Where(x => "123".IndexOf(x.OrderID.ToString()) == -1));

            AssertSql(
"""
SELECT `o`.`OrderID`, `o`.`CustomerID`, `o`.`EmployeeID`, `o`.`OrderDate`
FROM `Orders` AS `o`
WHERE (LOCATE(CAST(`o`.`OrderID` AS char), '123') - 1) = -1
""");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public virtual Task Datetime_subtraction_TotalDays(bool async)
            => AssertTranslationFailed(() => { var date = new DateTime(1997, 1, 1); return AssertQuery(async, ss => ss.Set<Order>().Where(o => o.OrderDate.HasValue && (o.OrderDate.Value - date).TotalDays > 365)); });

        [ConditionalFact]
        public virtual void Check_all_tests_overridden()
            => MySqlTestHelpers.AssertAllMethodsOverridden(GetType());

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        protected override void ClearLog()
            => Fixture.TestSqlLoggerFactory.Clear();
    }
}
