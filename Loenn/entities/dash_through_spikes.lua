local spikeHelper = require("helpers.spikes")

local handlers = $("Up", "Down", "Left", "Right"):map((dir -> spikeHelper.createEntityHandler("NerdHelper/DashThroughSpikes" .. dir, dir:lower(), false, false, {"Kalobi/NerdHelper/dashthroughspike"})))
for k, handler in pairs(handlers) do
    handler.placements[1].name = handler.placements[1].name:match("%a*")
end
return handlers
