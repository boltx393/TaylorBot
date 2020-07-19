﻿using Discord.Commands;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaylorBot.Net.Commands.Types
{
    public class Word
    {
        public string Value { get; }

        public Word(string value)
        {
            Value = value;
        }
    }

    public class WordTypeReader : TypeReader
    {
        public static Regex WhitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);

        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
        {
            if (WhitespaceRegex.IsMatch(input))
            {
                return Task.FromResult(TypeReaderResult.FromError(
                    CommandError.ParseFailed, "Input value can't contain spaces or line breaks."
                ));
            }
            else
            {
                return Task.FromResult(TypeReaderResult.FromSuccess(new Word(input)));
            }
        }
    }
}
