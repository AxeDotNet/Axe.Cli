using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Axe.Cli.Parser.Transformers;

namespace Axe.Cli.Parser
{
    /// <summary>
    /// Represents all args transformers.
    /// </summary>
    public class ArgsTransformers
    {
        public const string DefaultTransformerKey = "Default";
        public const string IntegerTransformerKey = "IntegerTransformer";

        static readonly Regex NamePattern = new Regex(
            "^[A-Z_][A-Z0-9_]*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        readonly Dictionary<string, ValueTransformer> registered =
            new Dictionary<string, ValueTransformer>(StringComparer.OrdinalIgnoreCase)
            {
                {DefaultTransformerKey, new DefaultTransformer()},
                {IntegerTransformerKey, new IntegerTransformer()}
            };

        internal ValueTransformer GetTransformer(string name)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }

            if (!registered.ContainsKey(name))
            {
                throw new ArgumentException($"The transformer '{name}' is not registered.");
            }

            return registered[name];
        }

        internal void RegisterTransformer(string name, ValueTransformer transformer)
        {
            void ValidateName()
            {
                if (name == null) { throw new ArgumentNullException(nameof(name)); }

                if (!NamePattern.IsMatch(name))
                {
                    throw new ArgumentException(
                        $"The name should only contains alphabet, digit and underscore and the first letter should not be digit: {name}");
                }
            }

            ValidateName();
            registered[name] = transformer ?? throw new ArgumentNullException(nameof(transformer));
        }

        static readonly ArgsTransformers Instance = new ArgsTransformers();

        /// <summary>
        /// Get the integer transformer to translate argument to a 32-bit integer. The key to the
        /// transformer is <see cref="IntegerTransformerKey"/>.
        /// </summary>
        public static ValueTransformer IntegerTransformer => Instance.GetTransformer(IntegerTransformerKey);

        /// <summary>
        /// Get the default transformer. The key to this transformer is
        /// <see cref="DefaultTransformerKey"/>.
        /// </summary>
        public static ValueTransformer Default => Instance.GetTransformer(DefaultTransformerKey);

        /// <summary>
        /// Register or replace a transformer to the transformer collection.
        /// </summary>
        /// <param name="name">The key to the transformer. The key is case insensitive.</param>
        /// <param name="transformer">The transformer instance.</param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> or <paramref name="transformer"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The name contains characters other than alphabet, digit and underscore. Or the first
        /// character is digit.
        /// </exception>
        public static void Register(string name, ValueTransformer transformer)
        {
            Instance.RegisterTransformer(name, transformer);
        }

        /// <summary>
        /// Get registered transformer.
        /// </summary>
        /// <param name="name">The key to the transformer. The key is case insensitive.</param>
        /// <returns>The transformer.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="name"/> is not a key to the registered transformers.
        /// </exception>
        public static ValueTransformer Get(string name)
        {
            return Instance.GetTransformer(name);
        }
    }
}