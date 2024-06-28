local shake = {}

shake.name = "NerdHelper/CutsceneShakeTrigger"
shake.placements = {
    name = "shake",
    data = {
        time = 0.3,
        xDirection = 0.0,
        yDirection = 0.0,
        frequency = "OnEachEntry"
    }
}
shake.fieldInformation = {
    frequency = {
        options = {
            {"On Each Entry", "OnEachEntry"},
            {"Once Per Room", "OncePerRoom"},
            {"Once Per Session", "OncePerSession"}
        },
        editable = false
    }
}

return shake