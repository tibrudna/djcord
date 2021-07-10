package main

import (
	"log"
	"os"
	"os/signal"
	"strings"
	"syscall"

	"github.com/bwmarrin/discordgo"
	"github.com/tibrudna/djcord/audio"
	"github.com/tibrudna/djcord/commands"
)

var commandService commands.CommandService

func main() {
	discord, err := discordgo.New("Bot " + os.Getenv("TOKEN"))
	if err != nil {
		log.Fatalf("Cannot use token: %s", err)
	}

	discord.AddHandler(messageCreate)

	commandService = commands.NewCommandService()
	installCommands()

	err = discord.Open()
	if err != nil {
		log.Fatalf("Cannot connect to discord: %s", err)
	}

	log.Println("Bot is now running")

	sc := make(chan os.Signal, 1)
	signal.Notify(sc, syscall.SIGINT, syscall.SIGTERM, os.Interrupt, os.Kill)
	<-sc

	discord.Close()
	log.Println("Bot has stopped")
}

func messageCreate(s *discordgo.Session, m *discordgo.MessageCreate) {
	if m.Author.Bot {
		return
	}

	if !strings.HasPrefix(m.Content, "!") {
		return
	}

	ctx := commands.Context{
		Session: s,
		Message: m,
	}

	commandService.ExecuteCommand(&ctx)
}

func installCommands() {
	commandService.AddCommand(&commands.Command{
		Name:     "ping",
		Executer: commands.PingCommand,
	})

	commandService.AddCommand(&commands.Command{
		Name:     "add",
		Executer: audio.AddSong,
	})

	commandService.AddCommand(&commands.Command{
		Name:     "join",
		Executer: audio.Join,
	})

	commandService.AddCommand(&commands.Command{
		Name:     "play",
		Executer: audio.Play,
	})
}
