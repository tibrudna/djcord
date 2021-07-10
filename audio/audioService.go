package audio

import (
	"log"
	"strings"

	"github.com/jonas747/dca"
	"github.com/kkdai/youtube/v2"
	"github.com/tibrudna/djcord/commands"
)

var player AudioPlayer = AudioPlayer{}
var loader AudioLoader = AudioLoader{}
var scheduler []string

func AddSong(ctx *commands.Context) {
	content := strings.Trim(ctx.Message.Content, "!")
	url := strings.Split(content, " ")[1]

	scheduler = append(scheduler, url)

	ctx.Session.ChannelMessageSend(ctx.Message.ChannelID, "Song was added to the queue.")
	log.Printf("Added Song: %s", scheduler[len(scheduler)-1])
}

func Join(ctx *commands.Context) {
	guild, err := ctx.Session.State.Guild(ctx.Message.GuildID)
	if err != nil {
		log.Printf("Cannot find guild: %s", err)
		return
	}

	for _, vs := range guild.VoiceStates {
		if vs.UserID == ctx.Message.Author.ID {
			vc, err := ctx.Session.ChannelVoiceJoin(guild.ID, vs.ChannelID, false, true)
			if err != nil {
				log.Printf("Cannot connect to voice channel: %s", err)
				return
			}

			player.AudioSendHandler = &AudioSendHandler{
				Connection: vc,
				Options:    dca.StdEncodeOptions,
			}

			player.AudioSendHandler.Options.Bitrate = 96
			player.AudioSendHandler.Options.RawOutput = true
			player.AudioSendHandler.Options.Application = "lowdelay"

			log.Println("Connected to voice channel")
			return
		}
	}

	log.Println("Cannot find voice channel")
}

func GetPlaylist(ctx *commands.Context) {
	content := strings.Trim(ctx.Message.Content, "!")
	url := strings.Split(content, " ")[1]

	client := youtube.Client{}

	playlist, err := client.GetPlaylist(url)
	if err != nil {
		log.Printf("Cannot load playlist: %s", err)
		return
	}

	for k, v := range playlist.Videos {
		log.Printf("%d. %s - %s", k, v.Title, v.Author)
		scheduler = append(scheduler, v.ID)
	}
}

func Play(ctx *commands.Context) {
	for {
		if len(scheduler) == 0 {
			break
		}

		loader.client = youtube.Client{}
		nextSong := scheduler[0]
		scheduler = scheduler[1:]

		stream := loader.LoadSong(nextSong)
		player.Play(stream)
	}
}

func Next(ctx *commands.Context) {
	player.Next()
}
