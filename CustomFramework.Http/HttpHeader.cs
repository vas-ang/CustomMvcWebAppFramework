﻿using System;

using CustomFramework.Http.Exceptions;

namespace CustomFramework.Http
{
    public class HttpHeader
    {
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public static HttpHeader Parse(string line)
        {
            string[] tokens = line.Split(new string[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
            {
                throw new BadRequestException("Invalid header.");
            }

            return new HttpHeader(tokens[0], tokens[1]);
        }

        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}
