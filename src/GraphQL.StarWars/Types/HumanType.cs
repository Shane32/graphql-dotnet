using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.StarWars.Extensions;
using GraphQL.Types;

namespace GraphQL.StarWars.Types
{
    public class HumanType : ObjectGraphType<Human>
    {
        public HumanType(StarWarsData data)
        {
            Name = "Human";

            Field(h => h.Id).Description("The id of the human.");
            Field(h => h.Name, nullable: true).Description("The name of the human.");

            Field<ListGraphType<CharacterInterface>>(
                "friends",
                resolve: context => data.GetFriends(context.Source)
            );

            Connection<CharacterInterface>()
                .Name("friendsConnection")
                .Description("A list of a character's friends.")
                .Bidirectional()
                .Resolve(context => context.GetPagedResults<Human, StarWarsCharacter>(data, context.Source.Friends));

            Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");

            Field("appearsInFlags", source => source.AppearsInFlags.FromFlags());
            //Field("appearsInFlags", source => source.AppearsInFlags.FromFlags(), type: typeof(ListGraphType<NonNullGraphType<AppearsInEnumGraphType>>));
            //Field<ListGraphType<NonNullGraphType<AppearsInEnumGraphType>>>("appearsInFlags",
            //    description: "Which movie they appear in.",
            //    resolve: context => ((AppearsInEnum?)context.Source.AppearsInFlags).FromFlags());

            Field(h => h.HomePlanet, nullable: true).Description("The home planet of the human.");

            Interface<CharacterInterface>();
        }
    }
    public static class EnumExtensions
    {
        public static IEnumerable<T> FromFlags<T>(this T value) where T : struct, Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Distinct()
                .Where(x => value.HasFlag(x));
        }

        public static IEnumerable<T> FromFlags<T>(this T? value) where T : struct, Enum
        {
            if (!value.HasValue)
                return null;

            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Distinct()
                .Where(x => value.Value.HasFlag(x));
        }

        public static T? CombineFlags<T>(this IEnumerable<T> values) where T : struct, Enum
        {
            switch (values)
            {
                case null:
                    return null;
                case IEnumerable<int> list:
                    return (T)(object)list.Aggregate((a, b) => a | b);
                case IEnumerable<uint> list:
                    return (T)(object)list.Aggregate((a, b) => a | b);
                case IEnumerable<long> list:
                    return (T)(object)list.Aggregate((a, b) => a | b);
                case IEnumerable<ulong> list:
                    return (T)(object)list.Aggregate((a, b) => a | b);
                default:
                    throw new NotSupportedException("Enum type not supported");
            }
        }
    }


}
