class CommentMailbox < ApplicationMailbox
  rescue_from(ActiveRecord::RecordNotFound) { bounced! }
  def process
    comment_id = mail.to.first[/\Acomment\+(.*)@/, 1]
    comment = Comment.find(comment_id)
    Comment.create!(post: comment.post, body: mail_body)
  end

private
  def mail_body
    if mail.multipart?
      mail.text_part.decoded
    else
      mail.decoded
    end
  end
end
