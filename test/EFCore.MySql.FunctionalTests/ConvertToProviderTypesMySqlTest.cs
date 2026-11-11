using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests
{
    public class ConvertToProviderTypesMySqlTest : ConvertToProviderTypesTestBase<ConvertToProviderTypesMySqlTest.ConvertToProviderTypesMySqlFixture>
    {
        protected class NullableBackedDataTypes
        {
            private short? _int16;
            private int? _int32;
            private long? _int64;
            private double? _double;
            private decimal? _decimal;
            private DateTime? _dateTime;
            private DateTimeOffset? _dateTimeOffset;
            private TimeSpan? _timeSpan;
            private DateOnly? _dateOnly;
            private TimeOnly? _timeOnly;
            private float? _single;
            private bool? _boolean;
            private byte? _byte;
            private ushort? _unsignedInt16;
            private uint? _unsignedInt32;
            private ulong? _unsignedInt64;
            private char? _character;
            private sbyte? _signedByte;
            private Enum64? _enum64;
            private Enum32? _enum32;
            private Enum16? _enum16;
            private Enum8? _enum8;
            private EnumU64? _enumU64;
            private EnumU32? _enumU32;
            private EnumU16? _enumU16;
            private EnumS8? _enumS8;

            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }

            public int PartitionId { get; set; }

            public short Int16
            {
                get => (short)_int16;
                set => _int16 = value;
            }

            public int Int32
            {
                get => (int)_int32;
                set => _int32 = value;
            }

            public long Int64
            {
                get => (long)_int64;
                set => _int64 = value;
            }

            public double Double
            {
                get => (double)_double;
                set => _double = value;
            }

            public decimal Decimal
            {
                get => (decimal)_decimal;
                set => _decimal = value;
            }

            public DateTime DateTime
            {
                get => (DateTime)_dateTime;
                set => _dateTime = value;
            }

            public DateTimeOffset DateTimeOffset
            {
                get => (DateTimeOffset)_dateTimeOffset;
                set => _dateTimeOffset = value;
            }

            public TimeSpan TimeSpan
            {
                get => (TimeSpan)_timeSpan;
                set => _timeSpan = value;
            }

            public DateOnly DateOnly
            {
                get => (DateOnly)_dateOnly;
                set => _dateOnly = value;
            }

            public TimeOnly TimeOnly
            {
                get => (TimeOnly)_timeOnly;
                set => _timeOnly = value;
            }

            public float Single
            {
                get => (float)_single;
                set => _single = value;
            }

            public bool Boolean
            {
                get => (bool)_boolean;
                set => _boolean = value;
            }

            public byte Byte
            {
                get => (byte)_byte;
                set => _byte = value;
            }

            public ushort UnsignedInt16
            {
                get => (ushort)_unsignedInt16;
                set => _unsignedInt16 = value;
            }

            public uint UnsignedInt32
            {
                get => (uint)_unsignedInt32;
                set => _unsignedInt32 = value;
            }

            public ulong UnsignedInt64
            {
                get => (ulong)_unsignedInt64;
                set => _unsignedInt64 = value;
            }

            public char Character
            {
                get => (char)_character;
                set => _character = value;
            }

            public sbyte SignedByte
            {
                get => (sbyte)_signedByte;
                set => _signedByte = value;
            }

            public Enum64 Enum64
            {
                get => (Enum64)_enum64;
                set => _enum64 = value;
            }

            public Enum32 Enum32
            {
                get => (Enum32)_enum32;
                set => _enum32 = value;
            }

            public Enum16 Enum16
            {
                get => (Enum16)_enum16;
                set => _enum16 = value;
            }

            public Enum8 Enum8
            {
                get => (Enum8)_enum8;
                set => _enum8 = value;
            }

            public EnumU64 EnumU64
            {
                get => (EnumU64)_enumU64;
                set => _enumU64 = value;
            }

            public EnumU32 EnumU32
            {
                get => (EnumU32)_enumU32;
                set => _enumU32 = value;
            }

            public EnumU16 EnumU16
            {
                get => (EnumU16)_enumU16;
                set => _enumU16 = value;
            }

            public EnumS8 EnumS8
            {
                get => (EnumS8)_enumS8;
                set => _enumS8 = value;
            }
        }

        protected class NonNullableBackedDataTypes
        {
            private short _int16;
            private int _int32;
            private long _int64;
            private double _double;
            private decimal _decimal;
            private DateTime _dateTime;
            private DateTimeOffset _dateTimeOffset;
            private TimeSpan _timeSpan;
            private DateOnly _dateOnly;
            private TimeOnly _timeOnly;
            private float _single;
            private bool _boolean;
            private byte _byte;
            private ushort _unsignedInt16;
            private uint _unsignedInt32;
            private ulong _unsignedInt64;
            private char _character;
            private sbyte _signedByte;
            private Enum64 _enum64;
            private Enum32 _enum32;
            private Enum16 _enum16;
            private Enum8 _enum8;
            private EnumU64 _enumU64;
            private EnumU32 _enumU32;
            private EnumU16 _enumU16;
            private EnumS8 _enumS8;

            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int Id { get; set; }

            public int PartitionId { get; set; }

            public short? Int16
            {
                get => _int16;
                set => _int16 = (short)value;
            }

            public int? Int32
            {
                get => _int32;
                set => _int32 = (int)value;
            }

            public long? Int64
            {
                get => _int64;
                set => _int64 = (long)value;
            }

            public double? Double
            {
                get => _double;
                set => _double = (double)value;
            }

            public decimal? Decimal
            {
                get => _decimal;
                set => _decimal = (decimal)value;
            }

            public DateTime? DateTime
            {
                get => _dateTime;
                set => _dateTime = (DateTime)value;
            }

            public DateTimeOffset? DateTimeOffset
            {
                get => _dateTimeOffset;
                set => _dateTimeOffset = (DateTimeOffset)value;
            }

            public TimeSpan? TimeSpan
            {
                get => _timeSpan;
                set => _timeSpan = (TimeSpan)value;
            }

            public DateOnly? DateOnly
            {
                get => _dateOnly;
                set => _dateOnly = (DateOnly)value;
            }

            public TimeOnly? TimeOnly
            {
                get => _timeOnly;
                set => _timeOnly = (TimeOnly)value;
            }

            public float? Single
            {
                get => _single;
                set => _single = (float)value;
            }

            public bool? Boolean
            {
                get => _boolean;
                set => _boolean = (bool)value;
            }

            public byte? Byte
            {
                get => _byte;
                set => _byte = (byte)value;
            }

            public ushort? UnsignedInt16
            {
                get => _unsignedInt16;
                set => _unsignedInt16 = (ushort)value;
            }

            public uint? UnsignedInt32
            {
                get => _unsignedInt32;
                set => _unsignedInt32 = (uint)value;
            }

            public ulong? UnsignedInt64
            {
                get => _unsignedInt64;
                set => _unsignedInt64 = (ulong)value;
            }

            public char? Character
            {
                get => _character;
                set => _character = (char)value;
            }

            public sbyte? SignedByte
            {
                get => _signedByte;
                set => _signedByte = (sbyte)value;
            }

            public Enum64? Enum64
            {
                get => _enum64;
                set => _enum64 = (Enum64)value;
            }

            public Enum32? Enum32
            {
                get => _enum32;
                set => _enum32 = (Enum32)value;
            }

            public Enum16? Enum16
            {
                get => _enum16;
                set => _enum16 = (Enum16)value;
            }

            public Enum8? Enum8
            {
                get => _enum8;
                set => _enum8 = (Enum8)value;
            }

            public EnumU64? EnumU64
            {
                get => _enumU64;
                set => _enumU64 = (EnumU64)value;
            }

            public EnumU32? EnumU32
            {
                get => _enumU32;
                set => _enumU32 = (EnumU32)value;
            }

            public EnumU16? EnumU16
            {
                get => _enumU16;
                set => _enumU16 = (EnumU16)value;
            }

            public EnumS8? EnumS8
            {
                get => _enumS8;
                set => _enumS8 = (EnumS8)value;
            }
        }

        public ConvertToProviderTypesMySqlTest(ConvertToProviderTypesMySqlFixture fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            fixture.TestSqlLoggerFactory.Clear();
            //fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override Task Can_perform_query_with_ansi_strings_test()
        {
            return Task.CompletedTask;
        }

        // TODO: Needed to customize:
        //          `private bool Equal(DateTimeOffset left, DateTimeOffset right)` Recheck/remove once fixed upstream.
        //       Recheck/remove after `https://github.com/dotnet/efcore/issues/26068` has been fixed upstream.
        #region https://github.com/dotnet/efcore/issues/26068

        [ConditionalFact]
        public virtual async Task Can_insert_and_read_back_all_non_nullable_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<BuiltInDataTypes>().Add(
                    new BuiltInDataTypes
                    {
                        Id = 1,
                        PartitionId = 1,
                        TestInt16 = -1234,
                        TestInt32 = -123456789,
                        TestInt64 = -1234567890123456789L,
                        TestDouble = -1.23456789,
                        TestDecimal = -1234567890.01M,
                        TestDateTime = DateTime.Parse("01/01/2000 12:34:56", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                        TestDateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TestTimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        TestSingle = -1.234F,
                        TestBoolean = true,
                        TestByte = 255,
                        TestUnsignedInt16 = 1234,
                        TestUnsignedInt32 = 1234565789U,
                        TestUnsignedInt64 = 1234567890123456789UL,
                        TestCharacter = 'a',
                        TestSignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, await context.SaveChangesAsync());
            }

            using (var context = CreateContext())
            {
                var dt = (await context.Set<BuiltInDataTypes>().Where(e => e.Id == 1).ToListAsync()).Single();

                var entityType = context.Model.FindEntityType(typeof(BuiltInDataTypes));
                AssertEqualIfMapped(entityType, (short)-1234, () => dt.TestInt16);
                AssertEqualIfMapped(entityType, -123456789, () => dt.TestInt32);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.TestInt64);
                AssertEqualIfMapped(entityType, -1.23456789, () => dt.TestDouble);
                AssertEqualIfMapped(entityType, -1234567890.01M, () => dt.TestDecimal);
                AssertEqualIfMapped(entityType, DateTime.Parse("01/01/2000 12:34:56", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                    () => dt.TestDateTime);
                AssertEqualIfMapped(
                    entityType, new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                    () => dt.TestDateTimeOffset);
                AssertEqualIfMapped(entityType, new TimeSpan(0, 10, 9, 8, 7), () => dt.TestTimeSpan);
                AssertEqualIfMapped(entityType, -1.234F, () => dt.TestSingle);
                AssertEqualIfMapped(entityType, true, () => dt.TestBoolean);
                AssertEqualIfMapped(entityType, (byte)255, () => dt.TestByte);
                AssertEqualIfMapped(entityType, Enum64.SomeValue, () => dt.Enum64);
                AssertEqualIfMapped(entityType, Enum32.SomeValue, () => dt.Enum32);
                AssertEqualIfMapped(entityType, Enum16.SomeValue, () => dt.Enum16);
                AssertEqualIfMapped(entityType, Enum8.SomeValue, () => dt.Enum8);
                AssertEqualIfMapped(entityType, (ushort)1234, () => dt.TestUnsignedInt16);
                AssertEqualIfMapped(entityType, 1234565789U, () => dt.TestUnsignedInt32);
                AssertEqualIfMapped(entityType, 1234567890123456789UL, () => dt.TestUnsignedInt64);
                AssertEqualIfMapped(entityType, 'a', () => dt.TestCharacter);
                AssertEqualIfMapped(entityType, (sbyte)-128, () => dt.TestSignedByte);
                AssertEqualIfMapped(entityType, EnumU64.SomeValue, () => dt.EnumU64);
                AssertEqualIfMapped(entityType, EnumU32.SomeValue, () => dt.EnumU32);
                AssertEqualIfMapped(entityType, EnumU16.SomeValue, () => dt.EnumU16);
                AssertEqualIfMapped(entityType, EnumS8.SomeValue, () => dt.EnumS8);
            }
        }

        [ConditionalFact]
        public virtual async Task Can_insert_and_read_back_non_nullable_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<NonNullableBackedDataTypes>().Add(
                    new NonNullableBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = true,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, await context.SaveChangesAsync());
            }

            using (var context = CreateContext())
            {
                var dt = (await context.Set<NonNullableBackedDataTypes>().Where(ndt => ndt.Id == 101).ToListAsync()).Single();

                var entityType = context.Model.FindEntityType(typeof(NonNullableBackedDataTypes));
                AssertEqualIfMapped(entityType, (short)-1234, () => dt.Int16);
                AssertEqualIfMapped(entityType, -123456789, () => dt.Int32);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.Int64);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.Int64);
                AssertEqualIfMapped(entityType, -1.23456789, () => dt.Double);
                AssertEqualIfMapped(entityType, -1234567890.01M, () => dt.Decimal);
                AssertEqualIfMapped(entityType, DateTime.Parse("01/01/2000 12:34:56"), () => dt.DateTime);
                AssertEqualIfMapped(
                    entityType, new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                    () => dt.DateTimeOffset);
                AssertEqualIfMapped(entityType, new TimeSpan(0, 10, 9, 8, 7), () => dt.TimeSpan);
                AssertEqualIfMapped(entityType, -1.234F, () => dt.Single);
                AssertEqualIfMapped(entityType, true, () => dt.Boolean);
                AssertEqualIfMapped(entityType, (byte)255, () => dt.Byte);
                AssertEqualIfMapped(entityType, Enum64.SomeValue, () => dt.Enum64);
                AssertEqualIfMapped(entityType, Enum32.SomeValue, () => dt.Enum32);
                AssertEqualIfMapped(entityType, Enum16.SomeValue, () => dt.Enum16);
                AssertEqualIfMapped(entityType, Enum8.SomeValue, () => dt.Enum8);
                AssertEqualIfMapped(entityType, (ushort)1234, () => dt.UnsignedInt16);
                AssertEqualIfMapped(entityType, 1234565789U, () => dt.UnsignedInt32);
                AssertEqualIfMapped(entityType, 1234567890123456789UL, () => dt.UnsignedInt64);
                AssertEqualIfMapped(entityType, 'a', () => dt.Character);
                AssertEqualIfMapped(entityType, (sbyte)-128, () => dt.SignedByte);
                AssertEqualIfMapped(entityType, EnumU64.SomeValue, () => dt.EnumU64);
                AssertEqualIfMapped(entityType, EnumU32.SomeValue, () => dt.EnumU32);
                AssertEqualIfMapped(entityType, EnumU16.SomeValue, () => dt.EnumU16);
                AssertEqualIfMapped(entityType, EnumS8.SomeValue, () => dt.EnumS8);
            }
        }

        [ConditionalFact]
        public virtual async Task Can_insert_and_read_back_nullable_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<NullableBackedDataTypes>().Add(
                    new NullableBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = false,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, await context.SaveChangesAsync());
            }

            using (var context = CreateContext())
            {
                var dt = (await context.Set<NullableBackedDataTypes>().Where(ndt => ndt.Id == 101).ToListAsync()).Single();

                var entityType = context.Model.FindEntityType(typeof(NullableBackedDataTypes));
                AssertEqualIfMapped(entityType, (short)-1234, () => dt.Int16);
                AssertEqualIfMapped(entityType, -123456789, () => dt.Int32);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.Int64);
                AssertEqualIfMapped(entityType, -1.23456789, () => dt.Double);
                AssertEqualIfMapped(entityType, -1234567890.01M, () => dt.Decimal);
                AssertEqualIfMapped(entityType, DateTime.Parse("01/01/2000 12:34:56"), () => dt.DateTime);
                AssertEqualIfMapped(
                    entityType, new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                    () => dt.DateTimeOffset);
                AssertEqualIfMapped(entityType, new TimeSpan(0, 10, 9, 8, 7), () => dt.TimeSpan);
                AssertEqualIfMapped(entityType, -1.234F, () => dt.Single);
                AssertEqualIfMapped(entityType, false, () => dt.Boolean);
                AssertEqualIfMapped(entityType, (byte)255, () => dt.Byte);
                AssertEqualIfMapped(entityType, Enum64.SomeValue, () => dt.Enum64);
                AssertEqualIfMapped(entityType, Enum32.SomeValue, () => dt.Enum32);
                AssertEqualIfMapped(entityType, Enum16.SomeValue, () => dt.Enum16);
                AssertEqualIfMapped(entityType, Enum8.SomeValue, () => dt.Enum8);
                AssertEqualIfMapped(entityType, (ushort)1234, () => dt.UnsignedInt16);
                AssertEqualIfMapped(entityType, 1234565789U, () => dt.UnsignedInt32);
                AssertEqualIfMapped(entityType, 1234567890123456789UL, () => dt.UnsignedInt64);
                AssertEqualIfMapped(entityType, 'a', () => dt.Character);
                AssertEqualIfMapped(entityType, (sbyte)-128, () => dt.SignedByte);
                AssertEqualIfMapped(entityType, EnumU64.SomeValue, () => dt.EnumU64);
                AssertEqualIfMapped(entityType, EnumU32.SomeValue, () => dt.EnumU32);
                AssertEqualIfMapped(entityType, EnumU16.SomeValue, () => dt.EnumU16);
                AssertEqualIfMapped(entityType, EnumS8.SomeValue, () => dt.EnumS8);
            }
        }

        [ConditionalFact]
        public override async Task Can_insert_and_read_back_object_backed_data_types()
        {
            using (var context = CreateContext())
            {
                context.Set<ObjectBackedDataTypes>().Add(
                    new ObjectBackedDataTypes
                    {
                        Id = 101,
                        PartitionId = 101,
                        String = "TestString",
                        Bytes = new byte[] { 10, 9, 8, 7, 6 },
                        Int16 = -1234,
                        Int32 = -123456789,
                        Int64 = -1234567890123456789L,
                        Double = -1.23456789,
                        Decimal = -1234567890.01M,
                        DateTime = DateTime.Parse("01/01/2000 12:34:56"),
                        DateTimeOffset = new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                        TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                        Single = -1.234F,
                        Boolean = false,
                        Byte = 255,
                        UnsignedInt16 = 1234,
                        UnsignedInt32 = 1234565789U,
                        UnsignedInt64 = 1234567890123456789UL,
                        Character = 'a',
                        SignedByte = -128,
                        Enum64 = Enum64.SomeValue,
                        Enum32 = Enum32.SomeValue,
                        Enum16 = Enum16.SomeValue,
                        Enum8 = Enum8.SomeValue,
                        EnumU64 = EnumU64.SomeValue,
                        EnumU32 = EnumU32.SomeValue,
                        EnumU16 = EnumU16.SomeValue,
                        EnumS8 = EnumS8.SomeValue
                    });

                Assert.Equal(1, await context.SaveChangesAsync());
            }

            using (var context = CreateContext())
            {
                var dt = (await context.Set<ObjectBackedDataTypes>().Where(ndt => ndt.Id == 101).ToListAsync()).Single();

                var entityType = context.Model.FindEntityType(typeof(ObjectBackedDataTypes));
                AssertEqualIfMapped(entityType, "TestString", () => dt.String);
                AssertEqualIfMapped(entityType, new byte[] { 10, 9, 8, 7, 6 }, () => dt.Bytes);
                AssertEqualIfMapped(entityType, (short)-1234, () => dt.Int16);
                AssertEqualIfMapped(entityType, -123456789, () => dt.Int32);
                AssertEqualIfMapped(entityType, -1234567890123456789L, () => dt.Int64);
                AssertEqualIfMapped(entityType, -1.23456789, () => dt.Double);
                AssertEqualIfMapped(entityType, -1234567890.01M, () => dt.Decimal);
                AssertEqualIfMapped(entityType, DateTime.Parse("01/01/2000 12:34:56"), () => dt.DateTime);
                AssertEqualIfMapped(
                    entityType, new DateTimeOffset(DateTime.Parse("01/01/2000 12:34:56"), TimeSpan.FromHours(-8.0)),
                    () => dt.DateTimeOffset);
                AssertEqualIfMapped(entityType, new TimeSpan(0, 10, 9, 8, 7), () => dt.TimeSpan);
                AssertEqualIfMapped(entityType, -1.234F, () => dt.Single);
                AssertEqualIfMapped(entityType, false, () => dt.Boolean);
                AssertEqualIfMapped(entityType, (byte)255, () => dt.Byte);
                AssertEqualIfMapped(entityType, Enum64.SomeValue, () => dt.Enum64);
                AssertEqualIfMapped(entityType, Enum32.SomeValue, () => dt.Enum32);
                AssertEqualIfMapped(entityType, Enum16.SomeValue, () => dt.Enum16);
                AssertEqualIfMapped(entityType, Enum8.SomeValue, () => dt.Enum8);
                AssertEqualIfMapped(entityType, (ushort)1234, () => dt.UnsignedInt16);
                AssertEqualIfMapped(entityType, 1234565789U, () => dt.UnsignedInt32);
                AssertEqualIfMapped(entityType, 1234567890123456789UL, () => dt.UnsignedInt64);
                AssertEqualIfMapped(entityType, 'a', () => dt.Character);
                AssertEqualIfMapped(entityType, (sbyte)-128, () => dt.SignedByte);
                AssertEqualIfMapped(entityType, EnumU64.SomeValue, () => dt.EnumU64);
                AssertEqualIfMapped(entityType, EnumU32.SomeValue, () => dt.EnumU32);
                AssertEqualIfMapped(entityType, EnumU16.SomeValue, () => dt.EnumU16);
                AssertEqualIfMapped(entityType, EnumS8.SomeValue, () => dt.EnumS8);
            }
        }

        private void AssertEqualIfMapped<T>(IEntityType entityType, T expected, Expression<Func<T>> actualExpression)
        {
            if (entityType.FindProperty(((MemberExpression)actualExpression.Body).Member.Name) != null)
            {
                var actual = actualExpression.Compile()();
                var type = UnwrapNullableEnumType(typeof(T));
                if (IsSignedInteger(type))
                {
                    Assert.True(Equal(Convert.ToInt64(expected), Convert.ToInt64(actual)), $"Expected:\t{expected}\r\nActual:\t{actual}");
                }
                else if (IsUnsignedInteger(type))
                {
                    Assert.True(Equal(Convert.ToUInt64(expected), Convert.ToUInt64(actual)), $"Expected:\t{expected}\r\nActual:\t{actual}");
                }
                else if(type == typeof(DateTime))
                {
                    Assert.True(Equal((DateTime)(object)expected, (DateTime)(object)actual), $"Expected:\t{expected:O}\r\nActual:\t{actual:O}");
                }
                else if (type == typeof(DateTimeOffset))
                {
                    Assert.True(Equal((DateTimeOffset)(object)expected, (DateTimeOffset)(object)actual), $"Expected:\t{expected:O}\r\nActual:\t{actual:O}");
                }
                else
                {
                    Assert.Equal(expected, actual);
                }
            }
        }

        private static bool IsSignedInteger(Type type)
            => type == typeof(int)
               || type == typeof(long)
               || type == typeof(short)
               || type == typeof(sbyte);

        private static bool IsUnsignedInteger(Type type)
            => type == typeof(byte)
               || type == typeof(uint)
               || type == typeof(ulong)
               || type == typeof(ushort)
               || type == typeof(char);

        private bool Equal(long left, long right)
        {
            if (left >= 0
                && right >= 0)
            {
                return Equal((ulong)left, (ulong)right);
            }

            if (left < 0
                && right < 0)
            {
                return Equal((ulong)-left, (ulong)-right);
            }

            return false;
        }

        private bool Equal(ulong left, ulong right)
        {
            if (Fixture.IntegerPrecision < 64)
            {
                var largestPrecise = 1ul << Fixture.IntegerPrecision;
                while (left > largestPrecise)
                {
                    left >>= 1;
                    right >>= 1;
                }
            }

            return left == right;
        }

        private bool Equal(DateTime left, DateTime right)
            => left.Equals(right) && (!Fixture.PreservesDateTimeKind || left.Kind == right.Kind);

        private bool Equal(DateTimeOffset left, DateTimeOffset right)
            => left.Equals(right); // => left.EqualsExact(right);

        #endregion

        public class ConvertToProviderTypesMySqlFixture : ConvertToProviderTypesFixtureBase
        {
        protected override void OnModelCreating(ModelBuilder modelBuilder, DbContext context)
        {
            base.OnModelCreating(modelBuilder, context);

                modelBuilder.Entity<NullableBackedDataTypes>()
                    .HasData(
                        new NullableBackedDataTypes
                        {
                            Id = 13,
                            PartitionId = 1,
                            Int16 = -1234,
                            Int32 = -123456789,
                            Int64 = -1234567890123456789L,
                            Double = -1.23456789,
                            Decimal = -1234567890.01M,
                            DateTime = new DateTime(1973, 9, 3),
                            DateTimeOffset = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0)),
                            TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                            DateOnly = new DateOnly(2020, 3, 1),
                            TimeOnly = new TimeOnly(12, 30, 45, 123),
                            Single = -1.234F,
                            Boolean = true,
                            Byte = 255,
                            UnsignedInt16 = 1234,
                            UnsignedInt32 = 1234565789U,
                            UnsignedInt64 = 1234567890123456789UL,
                            Character = 'a',
                            SignedByte = -128,
                            Enum64 = Enum64.SomeValue,
                            Enum32 = Enum32.SomeValue,
                            Enum16 = Enum16.SomeValue,
                            Enum8 = Enum8.SomeValue,
                            EnumU64 = EnumU64.SomeValue,
                            EnumU32 = EnumU32.SomeValue,
                            EnumU16 = EnumU16.SomeValue,
                            EnumS8 = EnumS8.SomeValue
                        });

                modelBuilder.Entity<NonNullableBackedDataTypes>()
                    .HasData(
                        new NonNullableBackedDataTypes
                        {
                            Id = 13,
                            PartitionId = 1,
                            Int16 = -1234,
                            Int32 = -123456789,
                            Int64 = -1234567890123456789L,
                            Double = -1.23456789,
                            Decimal = -1234567890.01M,
                            DateTime = new DateTime(1973, 9, 3),
                            DateTimeOffset = new DateTimeOffset(new DateTime(), TimeSpan.FromHours(-8.0)),
                            TimeSpan = new TimeSpan(0, 10, 9, 8, 7),
                            DateOnly = new DateOnly(2020, 3, 1),
                            TimeOnly = new TimeOnly(12, 30, 45, 123),
                            Single = -1.234F,
                            Boolean = true,
                            Byte = 255,
                            UnsignedInt16 = 1234,
                            UnsignedInt32 = 1234565789U,
                            UnsignedInt64 = 1234567890123456789UL,
                            Character = 'a',
                            SignedByte = -128,
                            Enum64 = Enum64.SomeValue,
                            Enum32 = Enum32.SomeValue,
                            Enum16 = Enum16.SomeValue,
                            Enum8 = Enum8.SomeValue,
                            EnumU64 = EnumU64.SomeValue,
                            EnumU32 = EnumU32.SomeValue,
                            EnumU16 = EnumU16.SomeValue,
                            EnumS8 = EnumS8.SomeValue
                        });

        }

            public override bool StrictEquality => true;

            public override bool SupportsAnsi => true;

            public override bool SupportsUnicodeToAnsiConversion => false;

            public override bool SupportsLargeStringComparisons => true;

            protected override ITestStoreFactory TestStoreFactory => MySqlTestStoreFactory.Instance;
            public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ServiceProvider.GetRequiredService<ILoggerFactory>();

            public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
                => base.AddOptions(builder);

            public override bool SupportsBinaryKeys => true;

            public override DateTime DefaultDateTime => new DateTime();

            public override bool PreservesDateTimeKind => false;

            public override bool SupportsDecimalComparisons => true;
        }
    }
}
