package commands

import (
	"github.com/bwmarrin/discordgo"
)

type Command struct {
	Name     string
	Executer func(c *Context)
}

type Context struct {
	Session *discordgo.Session
	Message *discordgo.MessageCreate
}

func PingCommand(c *Context) {
	c.Session.ChannelMessageSend(c.Message.ChannelID, "Pong yo")
}
