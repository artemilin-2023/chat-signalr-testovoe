﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chat.Infrastructure.Data.Configurations.Convertors
{
    public class DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = default) :
        ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, kind), mappingHints
        )
    {
    }
}
