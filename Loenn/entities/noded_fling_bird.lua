local drawableSprite = require("structs.drawable_sprite")

local meta = require("meta")
local v = require("utils.version_parser")
local currentLoenn = meta.version >= v("0.8.0")
local drawableText
if currentLoenn then
    drawableText = require("structs.drawable_text")
end

local bird = {}

bird.name = "NerdHelper/NodedFlingBird"
bird.depth = -1
bird.nodeLineRenderType = "line"
if currentLoenn then
    function bird.sprite(room, entity)
        local sprite = drawableSprite.fromTexture("characters/bird/Hover04", entity)
        local text = drawableText.fromText("0", entity.x - 2, entity.y + 1)
        return {sprite, text}
    end
else
    bird.texture = "characters/bird/Hover04"
end
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

if currentLoenn then
    function bird.fieldInformation(entity)
        return {
            leftFlingingNodes = {
                fieldType = "list",
                elementOptions = {
                    validator = function(str)
                        if not string.match(str, "^%d+$") then
                            return false
                        end
                        local num = tonumber(str)
                        return num >= 0 and num <= #entity.nodes
                    end
                }
            }
        }
    end
end

if currentLoenn then
    function bird.nodeSprite(room, entity, node, nodeIndex)
        local sprite = drawableSprite.fromTexture("characters/bird/Hover04", node)
        local text = drawableText.fromText(nodeIndex, node.x - 2, node.y + 1)
        return {sprite, text}
    end
else
    function bird.nodeSprite(room, entity, node)
        local sprite = drawableSprite.fromTexture("characters/bird/Hover04", node)
        sprite:setColor({1.0, 1.0, 1.0, 0.5})
        return sprite
    end
end

return bird
