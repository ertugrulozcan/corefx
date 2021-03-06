// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Reflection.Metadata.Ecma335
{
    internal static class TypeDefOrRefTag
    {
        internal const int NumberOfBits = 2;
        internal const uint LargeRowSize = 0x00000001 << (16 - NumberOfBits);
        internal const uint TypeDef = 0x00000000;
        internal const uint TypeRef = 0x00000001;
        internal const uint TypeSpec = 0x00000002;
        internal const uint TagMask = 0x00000003;
        internal const uint TagToTokenTypeByteVector = TokenTypeIds.TypeDef >> 24 | TokenTypeIds.TypeRef >> 16 | TokenTypeIds.TypeSpec >> 8;
        internal const TableMask TablesReferenced =
          TableMask.TypeDef
          | TableMask.TypeRef
          | TableMask.TypeSpec;

        // inlining improves perf of the tight loop in FindSystemObjectTypeDef by 25%
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Handle ConvertToToken(uint typeDefOrRefTag)
        {
            uint tokenType = (TagToTokenTypeByteVector >> ((int)(typeDefOrRefTag & TagMask) << 3)) << TokenTypeIds.RowIdBitCount;
            uint rowId = (typeDefOrRefTag >> NumberOfBits);

            if (tokenType == 0 || (rowId & ~TokenTypeIds.RIDMask) != 0)
            {
                Handle.ThrowInvalidCodedIndex();
            }

            return new Handle(tokenType | rowId);
        }
    }
}