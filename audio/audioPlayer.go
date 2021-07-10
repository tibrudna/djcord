package audio

type AudioPlayer struct {
	AudioSendHandler *AudioSendHandler
}

func (p *AudioPlayer) Play(streamUrl string) {
	p.AudioSendHandler.Send(streamUrl)
}

func (p *AudioPlayer) Next() {
	//TODO:
}
