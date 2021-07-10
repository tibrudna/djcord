package audio

import (
	"io"
	"log"

	"github.com/bwmarrin/discordgo"
	"github.com/jonas747/dca"
)

type AudioSendHandler struct {
	Connection *discordgo.VoiceConnection
	Options    *dca.EncodeOptions
}

func (s *AudioSendHandler) Send(streamUrl string) {
	encodingSession, err := dca.EncodeFile(streamUrl, s.Options)
	if err != nil {
		log.Printf("Cannot create opus stream: %s", err)
		return
	}

	defer encodingSession.Cleanup()

	done := make(chan error)
	dca.NewStream(encodingSession, s.Connection, done)
	err = <-done

	if err != nil && err != io.EOF {
		log.Printf("Stream unexpectedly shut down: %s", err)
		return
	}
}
