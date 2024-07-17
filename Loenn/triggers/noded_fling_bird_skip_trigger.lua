local skip = {}

skip.name = "NerdHelper/NodedFlingBirdSkipTrigger"
skip.placements = {
    name = "skip",
    data = {
        birdID = -1,
        skipToNode = 0,
        takeDirectRoute = true
    }
}
skip.fieldInformation = {
    birdID = {
        fieldType = "integer"
    },
    skipToNode = {
        fieldType = "integer"
    }
}

return skip