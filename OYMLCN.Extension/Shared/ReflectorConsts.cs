﻿#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN
{
    public class ReflectorConsts
    {
        public static readonly Type ObjectType = typeof(object);
        public static readonly Type IDataReaderType = typeof(IDataReader);
        public static readonly Type IDataRecordType = typeof(IDataRecord);

        public static readonly object[] EmptyObjectArray = new object[0];
        public static readonly Type ObjectArrayType = typeof(object[]);
        public static readonly Type StringArrayType = typeof(string[]);
        public static readonly Type Int32ArrayType = typeof(int[]);

        public static readonly Type BoolType = typeof(bool);
        public static readonly Type FloatType = typeof(float);

        public static readonly Type ConvertType = typeof(Convert);
        public static readonly Type IEnumerableType = typeof(IEnumerable<>);
        public static readonly Type IEnumeratorType = typeof(IEnumerator);

        public static readonly Type IQueryableType = typeof(IQueryable<>);
        public static readonly Type ListType = typeof(List<>);
        public static readonly Type ListObjectType = typeof(List<object>);
        public static readonly Type ListIntType = typeof(List<int>);

        public static readonly Type EnumerableType = typeof(Enumerable);
        public static readonly Type QueryableType = typeof(Queryable);

        public static readonly Type ByteType = typeof(byte);
        public static readonly Type Int16Type = typeof(short);
        public static readonly Type Int32Type = typeof(int);
        public static readonly Type Int64Type = typeof(long);
        public static readonly Type DoubleType = typeof(double);
        public static readonly Type DecimalType = typeof(decimal);

        public static readonly Type NullableType = typeof(Nullable<>);
        public static readonly Type Int16NullableType = typeof(short?);

        public static readonly Type StringType = typeof(string);

        public static readonly Type DateTimeType = typeof(DateTime);
        public static readonly Type DateTimeNullableType = typeof(DateTime?);

        public static readonly Type TimeSpanType = typeof(TimeSpan);
        public static readonly Type CompilerGeneratedAttributeType = typeof(CompilerGeneratedAttribute);

        public static readonly MethodInfo OrderByMethod = QueryableType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "OrderBy" && x.GetParameters().Length == 2);
        public static readonly MethodInfo OrderByDescendingMethod = QueryableType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
        public static readonly MethodInfo ThenByMethod = QueryableType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "ThenBy" && x.GetParameters().Length == 2);
        public static readonly MethodInfo ThenByDescendingMethod = QueryableType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "ThenByDescending" && x.GetParameters().Length == 2);

        public static readonly MethodInfo StringContains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        public static readonly MethodInfo IEnumerableListContains = EnumerableType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "Contains" && x.GetParameters().Length == 2);

        public static readonly MethodInfo ConvertToDateTimeMethod = ConvertType.GetMethod("ToDateTime", new Type[] { ObjectType });
        public static readonly MethodInfo ConvertToStringMethod = ConvertType.GetMethod("ToString", new Type[] { ObjectType });
        public static readonly MethodInfo ConvertToInt32Method = ConvertType.GetMethod("ToInt32", new Type[] { ObjectType });
        public static readonly MethodInfo ConvertToBoolMethod = ConvertType.GetMethod("ToBool", new Type[] { ObjectType });
        public static readonly MethodInfo AddDaysMethod = DateTimeType.GetMethod("AddDays", new Type[] { DoubleType });

        public static readonly MethodInfo AddOfList = ListType.GetMethods().FirstOrDefault((MethodInfo x) => x.Name == "Add" && x.GetParameters().Length == 1);

        public static readonly MethodInfo GetStringOfIDataReader = IDataRecordType.GetMethod("GetString", new Type[] { Int32Type });
        public static readonly MethodInfo GetInt16OfIDataReader = IDataRecordType.GetMethod("GetInt16", new Type[] { Int32Type });
        public static readonly MethodInfo GetInt32OfIDataReader = IDataRecordType.GetMethod("GetInt32", new Type[] { Int32Type });
        public static readonly MethodInfo GetInt64OfIDataReader = IDataRecordType.GetMethod("GetInt64", new Type[] { Int32Type });
        public static readonly MethodInfo IsDBNullfIDataReader = IDataRecordType.GetMethod("IsDBNull", new Type[] { Int32Type });
        public static readonly MethodInfo GetBooleanOfIDataReader = IDataRecordType.GetMethod("GetBoolean", new Type[] { Int32Type });
        public static readonly MethodInfo GetDoubleOfIDataReader = IDataRecordType.GetMethod("GetDouble", new Type[] { Int32Type });
        public static readonly MethodInfo GetFloatOfIDataReader = IDataRecordType.GetMethod("GetFloat", new Type[] { Int32Type });
        public static readonly MethodInfo GetByteOfIDataReader = IDataRecordType.GetMethod("GetByte", new Type[] { Int32Type });
        public static readonly MethodInfo GetDecimalOfIDataReader = IDataRecordType.GetMethod("GetDecimal", new Type[] { Int32Type });
        public static readonly MethodInfo GetDateTimeOfIDataReader = IDataRecordType.GetMethod("GetDateTime", new Type[] { Int32Type });
        public static readonly MethodInfo GetOrdinalOfIDataReader = IDataRecordType.GetMethod("GetOrdinal", new Type[] { StringType });
        public static readonly MethodInfo ReadOfIDataReader = IDataReaderType.GetMethod("Read", Type.EmptyTypes);
        public static readonly PropertyInfo DatePropertyOfDateTime = DateTimeType.GetProperty("Date");
        public static readonly PropertyInfo FieldCountOfIDataReader = IDataRecordType.GetProperty("FieldCount");
    }
}
