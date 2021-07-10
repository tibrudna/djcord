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

func (s *AudioSendHandler) Send(streamUrl string, interrupt chan bool) {
	encodingSession, err := dca.EncodeFile(streamUrl, s.Options)
	if err != nil {
		log.Printf("Cannot create opus stream: %s", err)
		return
	}

	defer encodingSession.Cleanup()

	for {
		select {
		case <-interrupt:
			return
		default:
			buffer, err := encodingSession.OpusFrame()
			if len(buffer) == 0 || err == io.EOF {
				break
			}

			s.Connection.OpusSend <- buffer
		}
	}
}
