using Microsoft.Xna.Framework;
using Occasus.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Occasus.Core.Generation;
using Plane = Abyss.World.Universe.Plane;

namespace Abyss.World.Phases
{
    public class PhaseFactory : Factory<IPhase>, IPhaseFactory
    {
        private const string DataLocation = "Content\\Data\\phasedata.xml";

        private const string Qualifier = "Abyss.World.Phases.Concrete.";

        private const string AberthQualifier = Qualifier + "Aberth.";
        private const string ArgusQualifier = Qualifier + "Argus.";
        private const string DioninQualifier = Qualifier + "Dionin.";
        private const string PhobonQualifier = Qualifier + "Phobon.";
        private const string ValusQualifier = Qualifier + "Valus.";

        private static int phaseNumber = 0;

        private static readonly IDictionary<string, string> qualifiers = new Dictionary<string, string>
        {
            { Plane.Aberth.ToString(), AberthQualifier },
            { Plane.Argus.ToString(), ArgusQualifier },
            { Plane.Dionin.ToString(), DioninQualifier },
            { Plane.Phobon.ToString(), PhobonQualifier },
            { Plane.Valus.ToString(), ValusQualifier }
        };

        private static readonly IDictionary<string, int> depthRequirements = new Dictionary<string, int>();

        private static readonly IDictionary<int, List<string>> phasesByDifficulty = new Dictionary<int, List<string>>
                                                                                  {
                                                                                      { 0, new List<string>() },
                                                                                      { 1, new List<string>() },
                                                                                      { 2, new List<string>() },
                                                                                      { 3, new List<string>() },
                                                                                      { 4, new List<string>() },
                                                                                      { 5, new List<string>() }
                                                                                  };

        private static readonly IDictionary<string, string> qualifiedPhases = new Dictionary<string, string>();

        private static readonly IDictionary<string, int> difficultyByPhase = new Dictionary<string, int>();
        
        private static readonly IDictionary<string, List<string>> phasesByPlane = new Dictionary<string, List<string>>();

        private static readonly IList<string> phases = new List<string>();
        
        /// <summary>
        /// Generates a random <see cref="IPhase" />.
        /// </summary>
        /// <returns>
        /// A random <see cref="IPhase" />.
        /// </returns>
        public override IPhase GenerateRandom()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a <see cref="IPhase" /> corresponding to the given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A <see cref="IPhase" /> corresponding to the given identifier.
        /// </returns>
        public override IPhase GenerateById(string id)
        {
            var qualifiedName = qualifiedPhases[id];
            var difficulty = difficultyByPhase[qualifiedName];
            var score = depthRequirements[qualifiedName];
            var phase = (IPhase)Activator.CreateInstance(Type.GetType(qualifiedName), difficulty, score);
            return phase;
        }

        /// <summary>
        /// Gets the random phase.
        /// </summary>
        /// <param name="difficulty">The difficulty.</param>
        /// <returns>A random phase.</returns>
        public IPhase GenerateRandomByDifficulty(int difficulty)
        {
            // We want a range of phases that include the specified difficulty.
            var phaseSet = new List<string>();
            for (var i = 1; i <= difficulty; i++)
            {
                var p = phasesByDifficulty[difficulty];
                if (p.Any())
                {
                    phaseSet.Add(p[MathsHelper.Random(p.Count)]);
                }
            }

            var qualifiedName = phaseSet[MathsHelper.Random(phaseSet.Count)];
            var score = depthRequirements[qualifiedName];
            return (IPhase)Activator.CreateInstance(Type.GetType(qualifiedName), difficulty, score);
        }

        /// <summary>
        /// Cycles the phase.
        /// </summary>
        /// <param name="currentPhase">The current phase.</param>
        /// <returns></returns>
        public IPhase CyclePhase(string currentPhase)
        {
            var qualifiedName = phases[phaseNumber];
            phaseNumber = phases.Count - 1 == phaseNumber ? 0 : phaseNumber + 1;

            var difficulty = difficultyByPhase[qualifiedName];
            var score = depthRequirements[qualifiedName];
            var phase = (IPhase)Activator.CreateInstance(Type.GetType(qualifiedName), difficulty, score);
            return phase;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent()
        {
            var stream = TitleContainer.OpenStream(DataLocation);
            var doc = XDocument.Load(stream);

            foreach (var p in doc.Descendants("Phases").Descendants("Phase"))
            {
                // Check whether the phase is active or not. If it isn't, don't process it.
                if (p.Attribute("active") != null)
                {
                    if (!bool.Parse(p.Attribute("active").Value))
                    {
                        continue;
                    }
                }

                // Phase id.
                var phaseId = p.Attribute("id").Value;
                var planeId = p.Attribute("plane").Value;

                var difficulty = int.Parse(p.Attribute("difficulty").Value);

                // TODO: Re-enable this.
                //var scoreRequirement = int.Parse(p.Attribute("score").Value);

                // Get the qualified name for the phase.
                var qualifiedId = qualifiers[planeId] + phaseId;

                // Add the score requirements as needed.
                depthRequirements.Add(qualifiedId, MathsHelper.Random(20, 30));

                // Add to the phase collection by difficulty.
                phasesByDifficulty[difficulty].Add(qualifiedId);
                difficultyByPhase[qualifiedId] = difficulty;

                // Add to the overall lists.
                if (!phasesByPlane.ContainsKey(planeId))
                {
                    phasesByPlane.Add(planeId, new List<string>());
                }

                phasesByPlane[planeId].Add(qualifiedId);

                phases.Add(qualifiedId);
                qualifiedPhases.Add(phaseId, qualifiedId);
            }
        }
    }
}
