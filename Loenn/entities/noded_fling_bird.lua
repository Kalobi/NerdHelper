local drawableSprite = require("structs.drawable_sprite")

local bird = {}

bird.name = "NerdHelper/NodedFlingBird"
bird.depth = -1
bird.nodeLineRenderType = "line"
bird.texture = "characters/bird/Hover04"
bird.nodeLimits = {0, -1}
bird.nodeVisibility = "always"

bird.placements = {
    name = "bird",
    data = {
        leftFlingingNodes = "",
        doCameraZoom = true,
        waitForLightning = false
    }
}

function bird.nodeSprite(room, entity, node)
    local sprite = drawableSprite.fromTexture("characters/bird/Hover04", node)
    sprite:setColor({1.0, 1.0, 1.0, 0.5})
    return sprite
end


return bird