namespace F7s.Modell.Physical.Plonatology {
    public class Planetology {
        /*
         Generation
            General Planetology
                Quantity / Composition / Mass / Volume / Density / Gravity
                Solar Influx
                Age
            Rheology
                Probably best skip in favor of some elegant placeholder.
            Plate Tectonics
                Should be derived from Rheology, but as above, an elegant placeholder should do.
            Climatology
                Use solar influx as a proxy for average temperature.
                Generate general wind speed based on day/night and pole/equator temperature difference.
                Provide liquid or frozen water depending on temperature.
                If water is frozen everywhere, congratulations, it's an iceball, simplify and skip everything else.
                If water is liquid anywhere, start the water cycle. Let it evaporate and rain, gather it where the plates are lowest. Presto, oceans. Keep some water frozen in permafrost regions, and keep some underground depending in general.
                Depending on pole/equator temperature differences, generate ocean currents.
                Depending on how much water goes through the water cycle, generate clouds.
                Depending on wind speed and atmospheric water, move clouds around. Let them rain occasionally. Let them rain especially when they hit mountains.
            Erosion
                From wind and rain, cause erosion on land.
                    Let this carve riverbeds, too.
                Depending on possible tidal influences from moon or other body, cause tidal erosion near coasts.
                Cause sedimentation on ocean floor, near rivers, and especially past river deltas.
                Also gather rocks and scree where mountains crumble.
                Where erosion is especially strong, turn the surface into gravel, sand, dust, regolith.
            Biosphere
                No need to consider biogenesis. Assume that it's all terrestrial life, brought thither.
                Still, only settle life where it can sustain itself, and only the right kinds for each place.

        Division
            Much of the above can be handled globally, i.e., for the entire planet as one.
            But not all, obviously. Some things can be abstracted:
                Plates can simply be a coordinate with a radius, or several such glued together.
                Oceans can be congruent with oceanic plates.
                Mountains and rift valley could be either:
                    Simply a line defined by start and end point, or several strung together.
                    Purely defined by plate borders and motions.
                Rivers are actually difficult since they feed from multiple sources, but even then it suffices to define a handful, where they fork, and where their common delta lies.

         */
    }
}
