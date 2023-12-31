﻿using System;
using System.Collections.Generic;
using IrregularVerbs.Models.Components;

namespace IrregularVerbs.Factories;

public static class VolatileFormFactory
{
    private static readonly Dictionary<char, CombineOperation> Separators = new Dictionary<char, CombineOperation>()
    {
        {'&', CombineOperation.And},
        {'|', CombineOperation.Or},
        {'/', CombineOperation.Unknown},
        {',', CombineOperation.Unknown},
        {'\"', CombineOperation.Unknown},
    };

    public static CombineOperation GetCombineOperationBySeparator(char separator)
    {
        return Separators.TryGetValue(separator, out CombineOperation operation) ? operation : CombineOperation.None;
    }

    public static VolatileForm FromCombinedNotation(string sourceNotation)
    {
        Tuple<string, string> variants;
        CombineOperation combineOperation;

        if (ContainsSeparator(sourceNotation, out char separator))
        {
            variants = ToVariantsTuple(sourceNotation, separator);
            combineOperation = Separators[separator];
        }
        else
        {
            variants = ToVariantsTuple(sourceNotation);
            combineOperation = CombineOperation.None;
        }

        return new VolatileForm(variants, combineOperation);
    }

    public static bool ContainsSeparator(string sourceNotation)
    {
        return ContainsSeparator(sourceNotation, out char foundSeparator);
    }

    public static bool ContainsSeparator(string sourceNotation, out char foundSeparator)
    {
        foreach (var separator in Separators)
        {
            if (sourceNotation.Contains(separator.Key))
            {
                foundSeparator = separator.Key;
                return true;
            }
        }

        foundSeparator = ' ';
        return false;
    }
    
    private static Tuple<string, string> ToVariantsTuple(string sourceNotation)
    {
        return new Tuple<string, string>(FixedFormFactory.FromNotation(sourceNotation), string.Empty);
    }
    
    private static Tuple<string, string> ToVariantsTuple(string sourceNotation, char separator)
    {
        string[] variantsArray = sourceNotation.Split(separator);
        
        switch (variantsArray.Length)
        {
            case 2: return new Tuple<string, string>(
                FixedFormFactory.FromNotation(variantsArray[0]), 
                FixedFormFactory.FromNotation(variantsArray[1]));
            
            case 1: return new Tuple<string, string>(
                FixedFormFactory.FromNotation(variantsArray[0]), 
                string.Empty);
            
            default: return new Tuple<string, string>(
                string.Empty, 
                string.Empty);
        }
    }
}